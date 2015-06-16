using AForge.Imaging.Filters;
using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Math.Geometry;
using System.Windows.Forms;
using AForge.Math;
using Accord.Statistics.Testing;

namespace SimilarityFinder
{
    /// <summary>
    /// The image comparer for similarity
    /// </summary>
    public class ImageComparer
    {
        private static int IGNORE = 10;
        private static int SMOOTHNESSSCALE = 1;
        private static int COLORSCALE = 10;
        private static int SQUARES = 10;
        private static double POINT_EPS = 2;
        ImageData image1, image2;
        private double _geometricSimilarity;
        private double _histogramSimilarity;
        private double _smoothnessSimilarity;

        public int Threshold1 { get; set; }

        public int Threshold2 { get; set; }

        public double GeometricSimilarity
        {
            get
            {
                return _geometricSimilarity;
            }
        }

        public double HistogramSimilarity
        {
            get
            {
                return _histogramSimilarity;
            }
        }

        public double SmoothnessSimilarity
        {
            get
            {
                return _smoothnessSimilarity;
            }
        }

        /// <summary>
        /// Image Comparer Constructor.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        public ImageComparer(ImageData im1, ImageData im2)
        {
            image1 = im1;
            image2 = im2;
        }

        /// <summary>
        /// Compares the images.
        /// </summary>
        public void Compare()
        {
            Adjust();
            _histogramSimilarity  = CompareHistograms(image1.Image, image2.Image);
            _smoothnessSimilarity = CompareSmoothness(image1.Image, image2.Image);
            _geometricSimilarity = DetectObjects(image1, image2);
            image1.RestoreOriginal();
            image2.RestoreOriginal();
            //DetsectEdges();
        }

        private double CompareSmoothness(Bitmap im1, Bitmap im2)
        {
            double difference = DistanceImages(im1, im2, Math.Min(im1.Width, im2.Height) / SQUARES);
            difference = difference / im1.Width / im1.Height;
            double similarity = 1;
            while (difference > SMOOTHNESSSCALE)
            {
                difference = difference - SMOOTHNESSSCALE;
                similarity = similarity - 0.1;
            }
            return Math.Max(similarity, 0);
        }

        /// <summary>
        /// Deprecated. Detect edges on the picture and
        /// compare the resulted images.
        /// Not finished.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        private void DetectEdges(ImageData im1, ImageData im2)
        {
            Grayscale grayscale = new Grayscale(0.33,0.33,0.33);
            Threshold threshold = new Threshold(128);
            HomogenityEdgeDetector detector = new HomogenityEdgeDetector();
            var tmp = grayscale.Apply(im1.Image);
            im1.Image = detector.Apply(tmp);
            im2.Image = detector.Apply(grayscale.Apply(im2.Image));
            
        }

        /// <summary>
        /// Detect objects (circles and polygons) on the pictures and check
        /// them for similarities.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double DetectObjects(ImageData im1, ImageData im2)
        {
            Grayscale colorFilter = new Grayscale(0.33, 0.33, 0.33);

            im1.Image = colorFilter.Apply(im1.Image);

            im2.Image = colorFilter.Apply(im2.Image);


            Threshold tr = new Threshold(Threshold1);
            tr.ApplyInPlace(im1.Image);
            tr.ThresholdValue = Threshold2;
            tr.ApplyInPlace(im2.Image);
            Invert invert = new Invert();
            if (im1.Image.GetPixel(0, 0).B > 20)
            {
                invert.ApplyInPlace(im1.Image);
            }
            if (im2.Image.GetPixel(0, 0).B > 20)
            {
                invert.ApplyInPlace(im2.Image);
            }
            im1.Image = im1.Image.Clone(new Rectangle(0, 0, im1.Image.Width, im1.Image.Height), PixelFormat.Format24bppRgb);
            im2.Image = im2.Image.Clone(new Rectangle(0, 0, im2.Image.Width, im2.Image.Height), PixelFormat.Format24bppRgb);

            double result = CompareBlobs(im1, im2);
            //DetectShapes(im1.Image, false);
            //DetectShapes(im2.Image, false);

            return result;
        }

        /// <summary>
        /// Compare characteristic points on the images.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareCharacteristicPoints(UnmanagedImage im1, UnmanagedImage im2)
        {
            SusanCornersDetector susanDetector = new SusanCornersDetector();
            MoravecCornersDetector moravecDetector = new MoravecCornersDetector();
            var result1 = susanDetector.ProcessImage(im1);
            var result2 = susanDetector.ProcessImage(im2);

            if (result1.Count > result2.Count)
            {
                var tmp = result1;
                result1 = result2;
                result2 = tmp;
            }

            if (result1.Count == 0 && result2.Count == 0)
                return 1;
            double similarity = 0;

            int similarPoints = 0;
            for (int i = 0; i < result1.Count; i++)
            {
                double percX1 = (result1[i].X + 0.0) / im1.Width,
                    percY1 = (result1[i].Y + 0.0) / im1.Height;
                for (int j = 0; j < result2.Count; j++)
                {
                    double percX2 = (result2[j].X + 0.0) / im2.Width,
                        percY2 = (result2[j].Y + 0.0) / im2.Height;
                    if (Math.Abs(percX1 - percX2) < POINT_EPS
                        && Math.Abs(percY1 - percY2) < POINT_EPS)
                    {
                        similarPoints++;
                        break;
                    }
                }
            }

            double pointsProportion = Math.Sqrt((result1.Count + 0.0) / result2.Count);
            similarity = (pointsProportion * similarPoints) / result1.Count;
            return similarity;
        }

        /// <summary>
        /// Compare histograms of two images, using modulo distance.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareHistograms(Bitmap im1, Bitmap im2)
        {
            Grayscale filter = Grayscale.CommonAlgorithms.BT709;
            Bitmap im1Gray = filter.Apply(im1),
                   im2Gray = filter.Apply(im2);
            int max = 10;
            int width = im1.Width / max,
                height = im1.Height / max;
            double similarity = 0;
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    Rectangle rectangle = new Rectangle(width * i, height * j,
                        Math.Min(im1.Width - width * i, width), Math.Min(im1.Height - height * j, height));
                    if (rectangle.Width <= 0 || rectangle.Height <= 0)
                    {
                        continue;
                    }
                    Bitmap i1 = im1.Clone(rectangle, im1.PixelFormat),
                        i2 = im2.Clone(rectangle, im2.PixelFormat),
                        i1Gray = im1Gray.Clone(rectangle, im1Gray.PixelFormat),
                        i2Gray = im2Gray.Clone(rectangle, im2Gray.PixelFormat);
                    ImageStatistics stats1 = new ImageStatistics(i1);
                    ImageStatistics stats2 = new ImageStatistics(i2);
                    ImageStatistics stats1Gray = new ImageStatistics(i1Gray);
                    ImageStatistics stats2Gray = new ImageStatistics(i2Gray);
                    double diff = CompareHistograms(stats1.Red, stats2.Red);
                    diff = diff * CompareHistograms(stats1.Blue, stats2.Blue);
                    diff = diff * CompareHistograms(stats1.Green, stats2.Green);
                    diff = diff * CompareHistograms(stats1Gray.Gray, stats2Gray.Gray);
                    //ImageStatisticsHSL statss1 = new ImageStatisticsHSL(i1);
                    //ImageStatisticsHSL statss2 = new ImageStatisticsHSL(i2);
                    //double diff = CompareHistograms(statss1.Luminance, statss2.Luminance, statss1.PixelsCount);
                    //diff = diff * CompareHistograms(statss1.Saturation, statss1.Saturation, statss1.PixelsCount);
                    i1.Dispose();
                    i2.Dispose();
                    i1Gray.Dispose();
                    i2Gray.Dispose();
                    similarity += diff;
                }
            }
            return similarity / max / max;
        }

        /// <summary>
        /// Compare two histograms, using modulo distance.
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareHistograms(ContinuousHistogram h1, ContinuousHistogram h2, int pixelCount)
        {
            int distance = DistanceModulo(h1, h2);
            return Math.Max(0, 1 - (distance / COLORSCALE) / (pixelCount + 0.0));
        }

        /// <summary>
        /// Generate histogram representing statistical values of difference between two input histograms
        /// </summary>
        /// <param name="h1">Input histogram</param>
        /// <param name="h2">Input histogram</param>
        /// <returns>Histogram with statistical data concerning input histograms' abs difference</returns>
        private Histogram CalculateDifferenceHistogram(Histogram h1, Histogram h2)
        {
            int[] diffHist = new int[h1.Values.Length];
            for (int i = 0; i < h1.Values.Length; i++)
            {
                diffHist[i] = Math.Abs(h1.Values[i] - h2.Values[i]);
            }
            return new Histogram(diffHist);
        }

        /// <summary>
        /// Compare two histograms, using modulo distance.
        /// </summary>
        /// <param name="h1">Histogram to be compared</param>
        /// <param name="h2">Histogram to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareHistograms(Histogram h1, Histogram h2)
        {
            //double diffMax = Math.Abs(h1.Max - h2.Max);
            //double diffMin = Math.Abs(h1.Min - h2.Min);
            //double diffMean = Math.Abs(h1.Mean - h2.Mean);
            //double diffMedian = Math.Abs(h1.Median - h2.Median);
            double[] t1 = new double[h1.Values.Length];
            double[] t2 = new double[h1.Values.Length];
            for (int i = 0; i < h1.Values.Length; i++)
            {
                t1[i] = h1.Values[i];
                t2[i] = h2.Values[i];
            }
            int distance = DistanceModulo(h1, h2);
            return Math.Max(0, 1 - (distance / SMOOTHNESSSCALE) / h1.TotalCount);
        }

        /// <summary>
        /// Compare two images pixel by pixel.
        /// 1. Spits the image into squares
        /// 2. In every square, the pixes "on edges" are counted
        /// 3. Comparison between the sqares is made, based on how "edgy" they are
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <param name="squareSize">Size of the square</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private int DistanceImages(Bitmap b1, Bitmap b2, int squareSize)
        {
            int hDist = 0;
            for (int i = 0; i < b1.Width / squareSize; i++)
            {
                for (int j = 0; j < b1.Height / squareSize; j++)
                {
                    int diff1 = 0, diff2 = 0;
                    for (int k = 0; k < squareSize; k++)
                    {
                        int x = k + i * squareSize + 1;
                        if (x == 1)
                            continue;
                        if (x >= b1.Width - 1)
                            break;
                        for (int l = 0; l < squareSize; l++)
                        {
                            int y = l + j * squareSize + 1;
                            if (y == 1)
                                continue;
                            if (y >= b1.Height - 1)
                                break;
                            diff1 += GetDifferenceWithNeighbors(x, y, b1, SMOOTHNESSSCALE, IGNORE);
                            diff2 += GetDifferenceWithNeighbors(x, y, b2, SMOOTHNESSSCALE, IGNORE);
                        }
                    }
                    hDist += Math.Abs(diff1 - diff2);
                }
            }
            return hDist;
        }

        /// <summary>
        /// Calcutates the edge weight in pixel.
        /// </summary>
        /// <param name="i">width</param>
        /// <param name="j">height</param>
        /// <param name="b">bitmap</param>
        /// <param name="scale">Scaling the edge weight</param>
        /// <param name="ignore">Threshold for an edge to be detected</param>
        /// <returns></returns>
        private int GetDifferenceWithNeighbors(int i, int j, Bitmap b, int scale, int ignore)
        {
            int difference = 0;
            difference += Math.Abs(b.GetPixel(i, j).R - b.GetPixel(i - 1, j).R) / scale;
            difference += Math.Abs(b.GetPixel(i, j).R - b.GetPixel(i + 1, j).R) / scale;
            difference += Math.Abs(b.GetPixel(i, j).R - b.GetPixel(i, j - 1).R) / scale;
            difference += Math.Abs(b.GetPixel(i, j).R - b.GetPixel(i, j - 1).R) / scale;

            difference += Math.Abs(b.GetPixel(i, j).G - b.GetPixel(i - 1, j).G) / scale;
            difference += Math.Abs(b.GetPixel(i, j).G - b.GetPixel(i + 1, j).G) / scale;
            difference += Math.Abs(b.GetPixel(i, j).G - b.GetPixel(i, j - 1).G) / scale;
            difference += Math.Abs(b.GetPixel(i, j).G - b.GetPixel(i, j - 1).G) / scale;

            difference += Math.Abs(b.GetPixel(i, j).B - b.GetPixel(i - 1, j).B) / scale;
            difference += Math.Abs(b.GetPixel(i, j).B - b.GetPixel(i + 1, j).B) / scale;
            difference += Math.Abs(b.GetPixel(i, j).B - b.GetPixel(i, j - 1).B) / scale;
            difference += Math.Abs(b.GetPixel(i, j).B - b.GetPixel(i, j - 1).B) / scale;

            if (Math.Abs(difference) < ignore)
            {
                return 0;
            }

            return difference;
        }

        /// <summary>
        /// Calculates the modulo distance between two histograms.
        /// </summary>
        /// <param name="h1">histogram to be compared</param>
        /// <param name="h2">histogram to be compared</param>
        /// <returns>Integer distance, more or equals zero</returns>
        private int DistanceModulo(ContinuousHistogram h1, ContinuousHistogram h2)
        {
            int[] prefixSum = new int[h1.Values.Length];
            int[] tmp = new int[h1.Values.Length];
            int hDist = 0;
            prefixSum[0] = h1.Values[0] - h2.Values[0];
            for (int i = 1; i < prefixSum.Length; i++)
            {
                prefixSum[i] = prefixSum[i - 1] + h1.Values[i] - h2.Values[i];
                hDist += Math.Abs(prefixSum[i]);
            }
            for (; ; )
            {
                int d = prefixSum[0];
                for (int i = 1; i < prefixSum.Length; i++)
                {
                    if (prefixSum[i] > 0)
                    {
                        d = Math.Min(d, prefixSum[i]);
                    }
                }
                int hDist2 = 0;
                for (int i = 0; i < prefixSum.Length; i++)
                {
                    tmp[i] = prefixSum[i] - d;
                    hDist2 += Math.Abs(prefixSum[i]);
                }
                if (hDist2 < hDist)
                {
                    hDist = hDist2;
                    for (int i = 0; i < prefixSum.Length; i++)
                    {
                        prefixSum[i] = tmp[i];
                    }
                }
                else
                {
                    break;
                }
            }
            for (; ; )
            {
                int d = prefixSum[0];
                for (int i = 1; i < prefixSum.Length; i++)
                {
                    if (prefixSum[i] < 0)
                    {
                        d = Math.Max(d, prefixSum[i]);
                    }
                }
                int hDist2 = 0;
                for (int i = 0; i < prefixSum.Length; i++)
                {
                    tmp[i] = prefixSum[i] - d;
                    hDist2 += Math.Abs(prefixSum[i]);
                }
                if (hDist2 < hDist)
                {
                    hDist = hDist2;
                    for (int i = 0; i < prefixSum.Length; i++)
                    {
                        prefixSum[i] = tmp[i];
                    }
                }
                else
                {
                    break;
                }
            }
            return hDist;
        }

        /// <summary>
        /// Calculates the modulo distance between two histograms.
        /// </summary>
        /// <param name="h1">histogram to be compared</param>
        /// <param name="h2">histogram to be compared</param>
        /// <returns>Integer distance, more or equals zero</returns>
        private int DistanceModulo(Histogram h1, Histogram h2)
        {
            int[] prefixSum = new int[h1.Values.Length];
            int[] tmp = new int[h1.Values.Length];
            int hDist = 0;
            prefixSum[0] = h1.Values[0] - h2.Values[0];
            for (int i = 1; i < prefixSum.Length; i++)
            {
                prefixSum[i] = prefixSum[i - 1] + h1.Values[i] - h2.Values[i];
                hDist += Math.Abs(prefixSum[i]);
            }
            for (; ; )
            {
                int d = prefixSum[0];
                for (int i = 1; i < prefixSum.Length; i++)
                {
                    if (prefixSum[i] > 0)
                    {
                        d = Math.Min(d, prefixSum[i]);
                    }
                }
                int hDist2 = 0;
                for (int i = 0; i < prefixSum.Length; i++)
                {
                    tmp[i] = prefixSum[i] - d;
                    hDist2 += Math.Abs(prefixSum[i]);
                }
                if (hDist2 < hDist)
                {
                    hDist = hDist2;
                    for (int i = 0; i < prefixSum.Length; i++)
                    {
                        prefixSum[i] = tmp[i];
                    }
                }
                else
                {
                    break;
                }
            }
            for (; ; )
            {
                int d = prefixSum[0];
                for (int i = 1; i < prefixSum.Length; i++)
                {
                    if (prefixSum[i] < 0)
                    {
                        d = Math.Max(d, prefixSum[i]);
                    }
                }
                int hDist2 = 0;
                for (int i = 0; i < prefixSum.Length; i++)
                {
                    tmp[i] = prefixSum[i] - d;
                    hDist2 += Math.Abs(prefixSum[i]);
                }
                if (hDist2 < hDist)
                {
                    hDist = hDist2;
                    for (int i = 0; i < prefixSum.Length; i++)
                    {
                        prefixSum[i] = tmp[i];
                    }
                }
                else
                {
                    break;
                }
            }
            return hDist;
        }

        /// <summary>
        /// Adjust the size of the images.
        /// </summary>
        private void Adjust()
        {
            int height = Math.Max(image1.Height, image2.Height);
            int width = Math.Max(image1.Width, image2.Width);
            ResizeBicubic filter = new ResizeBicubic(width, height);
            image1.Image = filter.Apply(image1.Image);
            image2.Image = filter.Apply(image2.Image);
        }

        /// <summary>
        /// Detect the shapes on the picture.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="edgePoints">Edge points for Blobs (they will be filled)</param>
        /// <returns>List of shapes (blobs)</returns>
        private Blob[] DetectBasicShapes(Bitmap bitmap, ref List<List<IntPoint>> edgePoints)
        {
            // lock image
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            BlobCounter blobCounter = new BlobCounter
            {
                FilterBlobs = true,
                MinHeight = bitmap.Height/50,
                MinWidth = bitmap.Width/50
            };


            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                edgePoints.Add(blobCounter.GetBlobsEdgePoints(blobs[i]));
            }
            return blobs;
        }

        /// <summary>
        /// Detect and compare blobs on two pictures
        /// </summary>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareBlobs(ImageData im1, ImageData im2)
        {
            List<List<IntPoint>> edgePoints1 = new List<List<IntPoint>>();
            List<List<IntPoint>> edgePoints2 = new List<List<IntPoint>>();
            var blobs1 = DetectBasicShapes(im1.Image, ref edgePoints1);
            var blobs2 = DetectBasicShapes(im2.Image, ref edgePoints2);

            return CompareBlobs2(blobs1, edgePoints1, blobs2, edgePoints2, im1.Image, im2.Image);
        }

        /// <summary>
        /// Compare two list of blobs :)
        /// </summary>
        /// <param name="blobs">First list of blobs</param>
        /// <param name="edgePoints">Points on blobs edges</param>
        /// <param name="otherBlobs">Second list of blobs</param>
        /// <param name="otherEdgePoints">Points on blobs edges (second list)</param>
        /// <param name="im1">Image to be compared</param>
        /// <param name="im2">Image to be compared</param>
        /// <returns>Similarity from 0.0 to 1.0</returns>
        private double CompareBlobs2(Blob[] blobs, List<List<IntPoint>> edgePoints,
                                    Blob[] otherBlobs, List<List<IntPoint>> otherEdgePoints,
                                    Bitmap im1, Bitmap im2)
        {
            GeometricalObject[] g1 = GetObjects(blobs, edgePoints, im1);
            GeometricalObject[] g2 = GetObjects(otherBlobs, otherEdgePoints, im2);
            double similarity = 0;
            foreach (var g in g1)
            {
                double currentSimilarity = 0,
                    currentMaxSimilarity = 0;
                foreach (var gg in g2)
                {
                    if (g.RoughlyEquals(gg))
                    {
                        currentSimilarity = CompareCharacteristicPoints(g.Image, gg.Image);
                        if (currentMaxSimilarity < currentSimilarity)
                        {
                            currentMaxSimilarity = currentSimilarity;
                        }
                    }
                }
                similarity += currentMaxSimilarity;
            }
            if (Math.Min(g1.Length, g2.Length) == 0)
            {
                return 0;
            }
            return similarity / Math.Min(g1.Length, g2.Length);
        }

        /// <summary>
        /// Get geometrical objects list from blobs
        /// </summary>
        /// <param name="blobs">List of blobs</param>
        /// <param name="edgePoints"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        private GeometricalObject[] GetObjects(Blob[] blobs, List<List<IntPoint>> edgePoints, Bitmap image = null)
        {
            GeometricalObject[] result = new GeometricalObject[blobs.Length];
            SimpleShapeChecker checker = new SimpleShapeChecker();
            AForge.Point center;
            float radius;
            for (int i = 0; i < blobs.Length; i++)
            {
                List<IntPoint> corners;
                if (checker.IsCircle(edgePoints[i], out center, out radius))
                {
                    result[i] = new Circle()
                    {
                        Points = edgePoints[i],
                        Area = blobs[i].Area,
                        Center = center,
                        Radius = radius
                    };
                }
                else
                {
                    checker.IsConvexPolygon(edgePoints[i], out corners);
                    result[i] = new Polygon()
                    {
                        Points = edgePoints[i],
                        Fullness = blobs[i].Fullness,
                        Area = blobs[i].Area,
                        Type = checker.CheckPolygonSubType(edgePoints[i]),
                        Corners = corners
                    };
                }
                if(image != null)
                {
                    result[i].Image = UnmanagedImage.FromManagedImage(image.Clone(blobs[i].Rectangle, image.PixelFormat));
                }
            }
            return result;
        }

        /// <summary>
        /// Detect and show the shapes on the bitmap.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="prepareBitmap">Does the bitmap needs to be changed</param>
        private void DetectShapes(Bitmap bitmap, bool prepareBitmap = true)
        {
            // lock image
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // step 1 - turn background to black
            if (prepareBitmap)
            {
                ColorFiltering colorFilter = new ColorFiltering
                {
                    Red = new IntRange(0, 64),
                    Green = new IntRange(0, 64),
                    Blue = new IntRange(0, 64),
                    FillOutsideRange = false
                };


                colorFilter.ApplyInPlace(bitmapData);
            }

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Graphics g = Graphics.FromImage(bitmap);
            Pen yellowPen = new Pen(Color.Yellow, 2); // circles
            Pen redPen = new Pen(Color.Red, 2);       // quadrilateral
            Pen brownPen = new Pen(Color.Brown, 2);   // quadrilateral with known sub-type
            Pen greenPen = new Pen(Color.Green, 2);   // known triangle
            Pen bluePen = new Pen(Color.Blue, 2);     // triangle

            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);

                AForge.Point center;
                float radius;

                // is circle ?
                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    g.DrawEllipse(yellowPen,
                        (float)(center.X - radius), (float)(center.Y - radius),
                        (float)(radius * 2), (float)(radius * 2));
                }
                else
                {
                    List<IntPoint> corners;

                    // is triangle or quadrilateral
                    if (shapeChecker.IsConvexPolygon(edgePoints, out corners))
                    {
                        // get sub-type
                        PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);

                        Pen pen;

                        if (subType == PolygonSubType.Unknown)
                        {
                            pen = (corners.Count == 4) ? redPen : bluePen;
                        }
                        else
                        {
                            pen = (corners.Count == 4) ? brownPen : greenPen;
                        }

                        g.DrawPolygon(pen, ToPointsArray(corners));
                    }
                    else
                    {
                        // get sub-type
                        PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);

                        Pen pen;

                        if (subType == PolygonSubType.Unknown)
                        {
                            pen = (corners.Count == 4) ? redPen : bluePen;
                        }
                        else
                        {
                            pen = (corners.Count == 4) ? brownPen : greenPen;
                        }

                        g.DrawPolygon(pen, ToPointsArray(corners));
                    }
                }
            }

            yellowPen.Dispose();
            redPen.Dispose();
            greenPen.Dispose();
            bluePen.Dispose();
            brownPen.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// Convers list of AForge.NET's points to array of .NET points
        /// </summary>
        /// <param name="points">Aforrge points</param>
        /// <returns>Drawing.Points</returns>
        private System.Drawing.Point[] ToPointsArray( List<IntPoint> points )
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for ( int i = 0, n = points.Count; i < n; i++ )
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }

        #region not used

        private void CompareBlobs(Blob[] blobs, List<List<IntPoint>> edgePoints,
                                    Blob[] otherBlobs, List<List<IntPoint>> otherEdgePoints)
        {
            SimpleShapeChecker checker = new SimpleShapeChecker();
            AForge.Point center;
            float radius;
            bool circle, polygon;
            PolygonSubType type;
            int k = 0;
            for (int i = 0; i < blobs.Length; i++)
            {
                List<IntPoint> corners;
                circle = checker.IsCircle(edgePoints[i], out center, out radius);
                if (!circle)
                {
                    polygon = checker.IsConvexPolygon(edgePoints[i], out corners);
                    type = checker.CheckPolygonSubType(edgePoints[i]);
                }
                else
                {
                    polygon = false;
                    type = PolygonSubType.Unknown;
                }

                for (int j = 0; j < otherBlobs.Length; j++)
                {
                    if (circle)
                    {
                        if (checker.IsCircle(otherEdgePoints[j], out center, out radius))
                        {
                            k++;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        List<IntPoint> otherCorners;
                        if (polygon == checker.IsConvexPolygon(otherEdgePoints[j], out otherCorners))
                        {
                            if (type == checker.CheckPolygonSubType(otherEdgePoints[j]))
                            {
                                k++;
                                break;
                            }
                        }
                    }
                }
            }
            if (k > Math.Min(blobs.Length, otherBlobs.Length) * 0.7)
            {
            }
        }

        private void GetCornersAndSizes(Blob[] blobs, List<List<IntPoint>> edgePoints, int[] cornersNum, double[] sizes)
        {
            SimpleShapeChecker checker = new SimpleShapeChecker();
            for (int i = 0; i < blobs.Length; i++)
            {
                AForge.Point center;
                float radius;
                sizes[i] = blobs[i].Area;
                if (checker.IsCircle(edgePoints[i], out center, out radius))
                {
                    cornersNum[i] = 0;
                }
                else
                {
                    List<IntPoint> corners;

                    checker.IsConvexPolygon(edgePoints[i], out corners);
                    cornersNum[i] = corners.Count;
                }
            }
        }

        #endregion
    }
}

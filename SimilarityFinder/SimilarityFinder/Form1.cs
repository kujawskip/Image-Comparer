using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimilarityFinder
{
    public partial class Form1 : Form
    {
        ImageData firstImage, secondImage;
        private int firstThreshold, secondThreshold;

        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            firstImage = secondImage = null;
        }

        private ImageData LoadImage()
        {
            ImageData imgData = null;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    imgData = new ImageData(ofd.FileName);

                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return imgData;
        }

        private void firstPictureLoadMenuItem_Click(object sender, EventArgs e)
        {
            firstImage = LoadImage();
            firstPicturePanel.BackgroundImage = firstImage.Image;
        }

        private void secondPictureLoadMenuItem_Click(object sender, EventArgs e)
        {
            secondImage = LoadImage();
            secondPicturePanel.BackgroundImage = secondImage.Image;
        }

        private void clearImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secondPicturePanel.BackgroundImage = 
                firstPicturePanel.BackgroundImage = null;
            if (firstImage != null)
                firstImage.Dispose();
            if (secondImage != null)
                secondImage.Dispose();
            firstImage = secondImage = null;
        }

        private void comparePicturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageComparer comparer = new ImageComparer(firstImage, secondImage);
            comparer.Threshold1 = firstThreshold;
            comparer.Threshold2 = secondThreshold;
            comparer.Compare();
            geometricSimilarityLabel.Text = String.Format("Geometrical similarity: {0}%", comparer.GeometricSimilarity * 100);
            smoothnessSimilarityLabel.Text = String.Format("Smoothness similarity: {0}%", comparer.SmoothnessSimilarity * 100);
            colorSimilarityLabel.Text = String.Format("Color similarity: {0}%", comparer.HistogramSimilarity * 100);
            firstPicturePanel.BackgroundImage = firstImage.Image;
            secondPicturePanel.BackgroundImage = secondImage.Image;

        }

        private void firstPictureThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdForm form = new ThresholdForm();
            form.Image = firstImage.Image;
            form.ShowDialog();
            firstThreshold = form.Threshold;
        }

        private void secondPictureThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdForm form = new ThresholdForm();
            form.Image = secondImage.Image;
            form.ShowDialog();
            secondThreshold = form.Threshold;
        }
    }
}

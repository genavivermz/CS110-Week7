using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace StoryGameApp;

public partial class MainWindow : Window
{
    // Each Slide keeps its text, image and description together.
    private record Slide(string Text, string ImagePath, string Description);
    private readonly Slide[] slides =
    {
        new Slide("Starting state", "Assets/Images/withered.jpeg", "Withered plants, sprinklers off."),
        new Slide("Action", "Assets/Images/door-closed.webp", "Exit greenhouse & shut door, high frequency pitch sounds."),
        new Slide("Result", "Assets/Images/watered.jpeg", "Frequency triggers plant watering system & toggles sprinklers.")
    };
    private int currentSlide = 0; // Array positions start at zero.
    private readonly Image[] slideImages;
    private readonly Border[] slideTiles;
    private readonly Rectangle[] slideDimmers;

    public MainWindow()
    {
        InitializeComponent(); // Build the named controls from XAML first.
        slideImages = new[] { StartingImage, ActionImage, ResultImage };
        slideTiles = new[] { StartingTile, ActionTile, ResultTile };
        slideDimmers = new[] { StartingDimmer, ActionDimmer, ResultDimmer };
        LoadSlideImages();
        ShowSlide(currentSlide);
    }

    private void LoadSlideImages()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, slides[i].ImagePath);
                if (!File.Exists(path)) throw new FileNotFoundException("Image file was not found.", path);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path, UriKind.Absolute);
                image.EndInit();
                slideImages[i].Source = image;
            }
            catch (Exception ex) when (ex is IOException || ex is NotSupportedException || ex is System.IO.FileFormatException)
            {
                slideImages[i].Source = null;
                StatusText.Text = $"Image unavailable for {slides[i].Text}. Check Assets/Images and the filename. {ex.Message}";
            }
        }
    }

    private void ShowSlide(int index)
    {
        if (index < 0 || index >= slides.Length) return;
        currentSlide = index;
        Slide slide = slides[currentSlide];
        StoryTextBlock.Text = slide.Text;
        ImageDescription.Text = slide.Description;
        SlideCounter.Text = $"Slide {currentSlide + 1} of {slides.Length}";
        BackButton.IsEnabled = currentSlide > 0;
        NextButton.IsEnabled = currentSlide < slides.Length - 1;
        for (int i = 0; i < slides.Length; i++)
        {
            bool isSelected = i == currentSlide;
            slideImages[i].Opacity = isSelected ? 1 : 0.62;
            slideTiles[i].BorderBrush = isSelected
                ? new SolidColorBrush(Color.FromRgb(183, 213, 154))
                : new SolidColorBrush(Color.FromRgb(71, 100, 81));
            slideDimmers[i].Visibility = isSelected ? Visibility.Collapsed : Visibility.Visible;
        }
        StatusText.Text = slideImages[currentSlide].Source is null
            ? $"Image unavailable for {slide.Text}. Check Assets/Images and the filename."
            : "Story ready.";
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(currentSlide + 1);
    }
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(currentSlide - 1);
    }
    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(0);
    }
    // DAY 7B: paste CaptureHandlers.cs.txt HERE, inside these class braces.
}

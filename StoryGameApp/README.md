# Day 7B integration
Keep your 7A checkpoint. Copy CaptureService.cs beside MainWindow.xaml.cs.
Replace the empty <StackPanel x:Name="CapturePanel"/> in MainWindow.xaml with
<StackPanel x:Name="CapturePanel">, the CaptureControls.xaml contents, and </StackPanel>.
Paste CaptureHandlers.cs.txt inside MainWindow's final class brace at the DAY 7B comment.
Keep the existing using directives and namespace. Build before testing.
The helper captures the primary Windows display, including your visible story app.
It does not record video, capture all displays, or hide the story window.
Move the story app onto the primary display. Close private content first.

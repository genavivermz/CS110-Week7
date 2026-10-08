## :bust_in_silhouette:  (10/8/2026 2:26 PM)
Explain one method or propose one test for Capture, Save/Cancel. Clear, or keeping currentSlide unchanged. Do not rewrite the project. Name the file, method. input, output and result I should observe. Use fictional data only.
## :speech_balloon: GitHub Copilot
File: CaptureControlsViewModelTests.cs

Test method: Capture_Cancel_DoesNotChangeCurrentSlide

Framework: xUnit (can be adapted to MSTest/NUnit). Purpose: verify that when a user starts a capture but cancels (Save/Cancel -> Cancel), the view-model keeps CurrentSlide unchanged and no save/persist operation occurs.

Input (fictional):
- Initial CurrentSlide: Slide { Id = 7, Title = "Intro", Content = "Existing text" }
- Mocked capture service returns a cancelled result (Saved = false, FilePath = null).
- Mocked slide repository that tracks SaveSlide calls.

Test steps (arrange / act / assert):
1. Arrange
   - Create a mock ICameraService (or IMediaCapture) with CaptureAsync() returning a CaptureResult { Saved = false, FilePath = null } to simulate user cancelling after capture preview.
   - Create a mock ISlideRepository (or IDataStore) and set up a verifier that SaveSlide(...) is not called.
   - Instantiate CaptureControlsViewModel with those mocks.
   - Set viewModel.CurrentSlide = new Slide { Id = 7, Title = "Intro", Content = "Existing text" }.

2. Act
   - Trigger the capture flow that the UI would: await viewModel.CaptureCommand.ExecuteAsync(null) (or call viewModel.StartCapture() then handle completion).
   - Simulate the capture finishing with a cancel result (the mocked ICameraService already returns cancelled).

3. Assert (expected output/result)
   - viewModel.CurrentSlide.Id == 7
   - viewModel.CurrentSlide.Content == "Existing text"
   - mockSlideRepository.Verify(r => r.SaveSlide(It.IsAny<Slide>()), Times.Never)
   - if the view-model exposes an InMemoryMediaStore, assert it contains 0 new items
   - test runner shows the test as Passed

Example pseudocode (xUnit + Moq):

[Fact]
public async Task Capture_Cancel_DoesNotChangeCurrentSlide()
{
    // Arrange
    var initial = new Slide { Id = 7, Title = "Intro", Content = "Existing text" };
    var cameraMock = new Mock<ICameraService>();
    cameraMock
      .Setup(c => c.CaptureAsync())
      .ReturnsAsync(new CaptureResult { Saved = false, FilePath = null }); // user cancelled

    var repoMock = new Mock<ISlideRepository>();
    var vm = new CaptureControlsViewModel(cameraMock.Object, repoMock.Object);
    vm.CurrentSlide = initial;

    // Act
    await vm.CaptureCommand.ExecuteAsync(null); // or vm.StartCapture()

    // Assert
    Assert.Equal(7, vm.CurrentSlide.Id);
    Assert.Equal("Existing text", vm.CurrentSlide.Content);
    repoMock.Verify(r => r.SaveSlide(It.IsAny<Slide>()), Times.Never);
}

Observed result when running:
- Test runner: test passes.
- In-app behavior (if replicated manually): after pressing Capture then Cancel, the on-screen slide remains the original slide (Id 7) and no new media is attached or persisted.

Notes / adaptation:
- If your project uses different names (e.g., CaptureAsync returns an enum or throws on cancel), adapt the mock to match the real API (return Cancel status, null filepath, or throw a UserCancelledException).
- If the capture flow is UI-only, test the backing ViewModel / controller rather than the XAML control to keep this unit test deterministic.

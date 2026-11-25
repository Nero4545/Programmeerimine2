using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object);
        }

        private void SetupList(params BeerType[] items)
        {
            _apiClientMock
                .Setup(x => x.List())
                .ReturnsAsync(new Result<List<BeerType>> { Value = items.ToList() });
        }

        [Fact]
        public void Default_constructor_should_create_commands()
        {
            // Act
            var viewModel = new MainWindowViewModel();

            // Assert
            Assert.NotNull(viewModel.NewCommand);
            Assert.NotNull(viewModel.SaveCommand);
            Assert.NotNull(viewModel.DeleteCommand);
        }

        [Fact]
        public void SelectedItem_should_raise_property_changed()
        {
            // Arrange
            string? changedProperty = null;
            _viewModel.PropertyChanged += (sender, e) => changedProperty = e.PropertyName;
            var item = new BeerType { Id = 1, Name = "IPA" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Same(item, _viewModel.SelectedItem);
            Assert.Equal(nameof(MainWindowViewModel.SelectedItem), changedProperty);
        }

        [Fact]
        public async Task Load_should_fill_list()
        {
            // Arrange
            SetupList(new BeerType { Id = 1, Name = "IPA" }, new BeerType { Id = 2, Name = "Stout" });

            // Act
            await _viewModel.Load();

            // Assert
            Assert.Equal(2, _viewModel.Lists.Count);
        }

        [Fact]
        public async Task Load_should_handle_empty_value()
        {
            // Arrange
            _apiClientMock
                .Setup(x => x.List())
                .ReturnsAsync(new Result<List<BeerType>> { Value = null });

            // Act
            await _viewModel.Load();

            // Assert
            Assert.Empty(_viewModel.Lists);
        }

        [Fact]
        public async Task Load_should_call_on_error_when_api_fails()
        {
            // Arrange
            string? error = null;
            _viewModel.OnError = message => error = message;
            _apiClientMock
                .Setup(x => x.List())
                .ReturnsAsync(new Result<List<BeerType>> { Error = "Connection failed" });

            // Act
            await _viewModel.Load();

            // Assert
            Assert.Equal("Connection failed", error);
            Assert.Empty(_viewModel.Lists);
        }

        [Fact]
        public void NewCommand_should_create_new_item()
        {
            // Act
            Assert.True(_viewModel.NewCommand.CanExecute(null));
            _viewModel.NewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem!.Id);
            Assert.Single(_viewModel.Lists);
            Assert.Same(_viewModel.SelectedItem, _viewModel.Lists[0]);
        }

        [Fact]
        public void SaveCommand_should_not_execute_without_selected_item()
        {
            // Act
            var canExecute = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void SaveCommand_should_execute_with_selected_item()
        {
            // Arrange
            _viewModel.SelectedItem = new BeerType();

            // Act
            var canExecute = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void SaveCommand_should_save_item_and_reload_list()
        {
            // Arrange
            var item = new BeerType { Name = "IPA" };
            _viewModel.SelectedItem = item;
            _apiClientMock
                .Setup(x => x.Save(item))
                .ReturnsAsync(new Result())
                .Verifiable();
            SetupList(new BeerType { Id = 1, Name = "IPA" });

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Single(_viewModel.Lists);
        }

        [Fact]
        public async Task Save_should_do_nothing_without_selected_item()
        {
            // Act
            await _viewModel.Save();

            // Assert
            _apiClientMock.Verify(x => x.Save(It.IsAny<BeerType>()), Times.Never);
        }

        [Fact]
        public async Task Save_should_call_on_error_when_api_fails()
        {
            // Arrange
            string? error = null;
            _viewModel.OnError = message => error = message;
            var item = new BeerType { Name = "IPA" };
            _viewModel.SelectedItem = item;
            _apiClientMock
                .Setup(x => x.Save(item))
                .ReturnsAsync(new Result { Error = "Save failed" });

            // Act
            await _viewModel.Save();

            // Assert
            Assert.Equal("Save failed", error);
            _apiClientMock.Verify(x => x.List(), Times.Never);
        }

        [Fact]
        public void DeleteCommand_should_not_execute_without_selected_item()
        {
            // Act
            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void DeleteCommand_should_not_execute_for_new_item()
        {
            // Arrange
            _viewModel.SelectedItem = new BeerType { Id = 0 };

            // Act
            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void DeleteCommand_should_execute_for_existing_item()
        {
            // Arrange
            _viewModel.SelectedItem = new BeerType { Id = 1 };

            // Act
            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void DeleteCommand_should_delete_item_when_confirmed()
        {
            // Arrange
            var item = new BeerType { Id = 1, Name = "IPA" };
            _viewModel.SelectedItem = item;
            _viewModel.ConfirmDelete = _ => true;
            _apiClientMock
                .Setup(x => x.Delete(1))
                .ReturnsAsync(new Result())
                .Verifiable();
            SetupList();

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Null(_viewModel.SelectedItem);
        }

        [Fact]
        public async Task Delete_should_delete_item_without_confirm_delete()
        {
            // Arrange
            _viewModel.SelectedItem = new BeerType { Id = 1 };
            _apiClientMock
                .Setup(x => x.Delete(1))
                .ReturnsAsync(new Result())
                .Verifiable();
            SetupList();

            // Act
            await _viewModel.Delete();

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_not_delete_when_not_confirmed()
        {
            // Arrange
            var item = new BeerType { Id = 1 };
            _viewModel.SelectedItem = item;
            _viewModel.ConfirmDelete = _ => false;

            // Act
            await _viewModel.Delete();

            // Assert
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
            Assert.Same(item, _viewModel.SelectedItem);
        }

        [Fact]
        public async Task Delete_should_do_nothing_without_selected_item()
        {
            // Act
            await _viewModel.Delete();

            // Assert
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_on_error_when_api_fails()
        {
            // Arrange
            string? error = null;
            _viewModel.OnError = message => error = message;
            var item = new BeerType { Id = 1 };
            _viewModel.SelectedItem = item;
            _apiClientMock
                .Setup(x => x.Delete(1))
                .ReturnsAsync(new Result { Error = "Delete failed" });

            // Act
            await _viewModel.Delete();

            // Assert
            Assert.Equal("Delete failed", error);
            Assert.Same(item, _viewModel.SelectedItem);
        }
    }
}

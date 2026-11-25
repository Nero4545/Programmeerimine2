using KooliProjekt.WpfApp;
using KooliProjekt.WpfApp.Api;
using Xunit;

namespace KooliProjekt.WpfApp.UnitTests
{
    public class RelayCommandTests
    {
        [Fact]
        public void CanExecute_should_return_true_without_predicate()
        {
            // Arrange
            var command = new RelayCommand<BeerType>(_ => { });

            // Act & Assert
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void CanExecute_should_use_predicate()
        {
            // Arrange
            var command = new RelayCommand<BeerType>(_ => { }, item => item != null);

            // Act & Assert
            Assert.False(command.CanExecute(null));
            Assert.True(command.CanExecute(new BeerType()));
        }

        [Fact]
        public void Execute_should_pass_parameter_to_action()
        {
            // Arrange
            BeerType? received = null;
            var command = new RelayCommand<BeerType>(item => received = item);
            var parameter = new BeerType { Id = 5 };

            // Act
            command.Execute(parameter);

            // Assert
            Assert.Same(parameter, received);
        }

        [Fact]
        public void CanExecuteChanged_should_allow_subscribe_and_unsubscribe()
        {
            // Arrange
            var command = new RelayCommand<BeerType>(_ => { });
            EventHandler handler = (sender, e) => { };

            // Act
            command.CanExecuteChanged += handler;
            command.CanExecuteChanged -= handler;

            // Assert
            Assert.True(command.CanExecute(null));
        }
    }
}

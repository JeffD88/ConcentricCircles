namespace ConcentricCircles.ViewModel
{
    using ConcentricCircles.Commands;
    using ConcentricCircles.Services;

    using Mastercam.IO;
    using Mastercam.IO.Types;
    using Mastercam.Math;
    using System.ComponentModel;

    using System.Windows;
    using System.Windows.Input;

    public class MainViewViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private Point3D centerPoint;

        private double startDiameter;

        private double percentChange;

        private int numberOfInstances;

        private bool isCollapsing;

        private readonly Window view;

        private readonly IConcentricCircleService concentricCircle;

        #endregion

        #region Construction

        public MainViewViewModel(Window view, ConcentricCircleService concentricCircle)
        {
            this.view = view;
            this.concentricCircle = concentricCircle;

            this.SelectPoint = new DelegateCommand(this.OnSelectPoint);
            this.OKCommand = new DelegateCommand(this.OnOKCommand);
            this.CancelCommand = new DelegateCommand(this.OnCancelCommand);

            this.centerPoint = new Point3D(0.0, 0.0, 0.0);
            this.IsCollapsing = false;
        }

        #endregion

        #region Commands

        public ICommand SelectPoint { get; }
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        #region Public Properties

        public double StartDiameter
        {
            get => this.startDiameter;
            set
            {
                this.startDiameter = value;
                OnPropertyChanged(nameof(this.StartDiameter));
            }
        }

        public double PercentChange
        {
            get => this.percentChange;
            set
            {
                this.percentChange = value;
                OnPropertyChanged(nameof(this.PercentChange));
            }
        }

        public int NumberOfInstances
        {
            get => this.numberOfInstances;
            set
            {
                this.numberOfInstances = value;
                OnPropertyChanged(nameof(this.NumberOfInstances));
            }
        }

        public bool IsCollapsing
        {
            get => this.isCollapsing;
            set
            {
                this.isCollapsing = value;
                OnPropertyChanged(nameof(this.IsCollapsing));
            }
        }

        #endregion

        #region Private Methods

        private void OnSelectPoint(object parameter)
        {
            this.view.Hide();
            SelectionManager.AskForPoint("Select Center Point", PointMask.Null, ref centerPoint);
            PromptManager.Clear();
            this.view.ShowDialog();
        }

        private void OnOKCommand(object parameter)
        {
            if (IsCollapsing)
            {
                concentricCircle.DrawCollapsing(centerPoint, StartDiameter, PercentChange, NumberOfInstances);
            }               
            else             
                concentricCircle.DrawExpanding(centerPoint, StartDiameter, PercentChange, NumberOfInstances);

            this.view.Close();
        }

        private void OnCancelCommand(object parameter) => this.view?.Close();

        #endregion

        #region Notify Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion
    }
}
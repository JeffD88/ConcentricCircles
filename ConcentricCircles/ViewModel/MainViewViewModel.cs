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

        private int patternTypeIndex;

        private Point3D centerPoint;

        private double startDiameterX;

        private double startDiameterY;

        private bool yDiameterEnabled;

        private double percentChange;

        private int numberOfInstances;

        private bool isCollapsing;

        private readonly Window view;

        private readonly IConcentricPatternService concentricPattern;

        #endregion

        #region Construction

        public MainViewViewModel(Window view, ConcentricPatternService concentricPattern)
        {
            this.view = view;
            this.concentricPattern = concentricPattern;

            this.SelectPoint = new DelegateCommand(this.OnSelectPoint);
            this.OKCommand = new DelegateCommand(this.OnOKCommand);
            this.CancelCommand = new DelegateCommand(this.OnCancelCommand);

            this.PatternTypeIndex = 0;
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

        public int PatternTypeIndex
        {
            get => this.patternTypeIndex;

            set
            {
                if (value == 0)
                {
                    this.StartDiameterY = 0.0;
                    this.YDiameterEnabled = false;
                }
                else
                {
                    this.YDiameterEnabled = true;
                }

                this.patternTypeIndex = value;
                this.OnPropertyChanged(nameof(this.PatternTypeIndex));
            }
        }

        public double StartDiameterX
        {
            get => this.startDiameterX;
            set
            {
                this.startDiameterX = value;
                OnPropertyChanged(nameof(this.StartDiameterX));
            }
        }

        public double StartDiameterY
        {
            get => this.startDiameterY;
            set
            {
                this.startDiameterY = value;
                OnPropertyChanged(nameof(this.StartDiameterY));
            }
        }

        public bool YDiameterEnabled
        {
            get => this.yDiameterEnabled;
            set
            {
                this.yDiameterEnabled = value;
                OnPropertyChanged(nameof(this.YDiameterEnabled));
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
            if (PatternTypeIndex == 0)
            {
                if (IsCollapsing)               
                    concentricPattern.DrawCollapsing(centerPoint, StartDiameterX, PercentChange, NumberOfInstances);
                
                else
                    concentricPattern.DrawExpanding(centerPoint, StartDiameterX, PercentChange, NumberOfInstances);
            }
            else
            {
                if (IsCollapsing)
                    concentricPattern.DrawCollapsing(centerPoint, StartDiameterX, StartDiameterY, PercentChange, NumberOfInstances);

                else
                    concentricPattern.DrawExpanding(centerPoint, StartDiameterX, StartDiameterY, PercentChange, NumberOfInstances);
            }

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
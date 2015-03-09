namespace AdvancedResourceTab.Extension
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;

    using Timer = System.Timers.Timer;

    /// <summary>
    ///     Implementation of the debounce-pattern.
    ///     Class to handle event-execution, which will only be executed if a spezified time elapsed since the last
    ///     execution-request.
    ///     Feature-Idea: if event-polling is active, decide if the manager has to raise its event after the delay even if
    ///     polling, or only if polling finieshed for the given delay.
    ///     currently only if polling finished for the given delay.
    /// </summary>
    public class DebounceManager
    {
        #region Fields

        private Timer debouceTimer;

        private Dispatcher dispatcher;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DebounceManager" /> class.
        /// </summary>
        /// <param name="eventDelay">The event delay.</param>
        /// <param name="executingDispatcher">The executing dispatcher.</param>
        public DebounceManager(TimeSpan eventDelay, Dispatcher executingDispatcher)
            : this()
        {
            this.EventDelay = eventDelay;
            this.dispatcher = executingDispatcher;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DebounceManager" /> class.
        /// </summary>
        public DebounceManager()
        {
            this.debouceTimer = new Timer
                                 {
                                     // true = repeat timer, false = stop if time is over
                                     AutoReset = false,
                                 };

            this.EventDelay = TimeSpan.FromMilliseconds(1);
            this.dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            this.debouceTimer.Elapsed += (timer, args) => this.OnStartExecution();
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when the configured event-delay passed since the last execution-request.
        /// </summary>
        public event EventHandler StartExecution;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current dispatcher.
        /// </summary>
        /// <value>
        ///     The current dispatcher.
        /// </value>
        public static Dispatcher CurrentDispatcher
        {
            get
            {
                return Dispatcher.FromThread(Thread.CurrentThread);
            }
        }

        /// <summary>
        ///     Sets the event delay.
        /// </summary>
        /// <value>
        ///     The event delay.
        /// </value>
        public TimeSpan EventDelay
        {
            set
            {
                this.debouceTimer.Interval = value.TotalMilliseconds;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Requests an event execution.
        /// </summary>
        public void RequestEventExecution()
        {
            this.debouceTimer.Stop();
            this.debouceTimer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called to raise the <see cref="StartExecution" /> event.
        /// </summary>
        private void OnStartExecution()
        {
            if (this.StartExecution != null)
            {
                this.dispatcher.BeginInvoke((Action)(() => this.StartExecution(this, EventArgs.Empty)));
            }
        }

        #endregion
    }
}
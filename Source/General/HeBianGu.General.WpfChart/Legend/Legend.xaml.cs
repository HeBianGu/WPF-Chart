using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeBianGu.WPF.EChart
{
    /// <summary>
    /// Legend.xaml 的交互逻辑
    /// </summary>
    public partial class Legend : UserControl
    {
        public Legend()
        {
            InitializeComponent();
        }

        #region Position properties

        public double LegendLeft
        {
            get { return (double)GetValue(LegendLeftProperty); }
            set { SetValue(LegendLeftProperty, value); }
        }

        public static readonly DependencyProperty LegendLeftProperty = DependencyProperty.Register(
          "LegendLeft",
          typeof(double),
          typeof(Legend),
          new FrameworkPropertyMetadata(Double.NaN));

        public double LegendRight
        {
            get { return (double)GetValue(LegendRightProperty); }
            set { SetValue(LegendRightProperty, value); }
        }

        public static readonly DependencyProperty LegendRightProperty = DependencyProperty.Register(
          "LegendRight",
          typeof(double),
          typeof(Legend),
          new FrameworkPropertyMetadata(10.0));

        public double LegendBottom
        {
            get { return (double)GetValue(LegendBottomProperty); }
            set { SetValue(LegendBottomProperty, value); }
        }

        public static readonly DependencyProperty LegendBottomProperty = DependencyProperty.Register(
          "LegendBottom",
          typeof(double),
          typeof(Legend),
          new FrameworkPropertyMetadata(Double.NaN));

        public double LegendTop
        {
            get { return (double)GetValue(LegendTopProperty); }
            set { SetValue(LegendTopProperty, value); }
        }

        public static readonly DependencyProperty LegendTopProperty = DependencyProperty.Register(
          "LegendTop",
          typeof(double),
          typeof(Legend),
          new FrameworkPropertyMetadata(10.0));

        #endregion

        /// <summary>
        /// Adds new legend item.
        /// </summary>
        /// <param name="legendItem">The legend item.</param>
        public void AddLegendItem(LegendItem legendItem)
        {
            stackPanel.Children.Add(legendItem);
        }
    }

    /// <summary>
    /// <see cref="LegendItem"/> is a base class for item in legend, that represents some chart. 
    /// </summary>
    public abstract class LegendItem : CheckBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        protected LegendItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        protected LegendItem(Description description)
        {
            Description = description;
        }

        private Description description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public Description Description
        {
            get { return description; }
            set
            {
                description = value;
                //Content = description;
            }
        }
    }



    #region - Description -


    public class ResolveLegendItemEventArgs : EventArgs
    {
        public ResolveLegendItemEventArgs(LegendItem legendItem)
        {
            LegendItem = legendItem;
        }

        public LegendItem LegendItem { get; set; }
    }

    public abstract class Description
    {

        private LegendItem legendItem;
        public LegendItem LegendItem
        {
            get
            {
                if (legendItem == null)
                {
                    legendItem = CreateLegendItem();
                }
                return legendItem;
            }
        }

        private LegendItem CreateLegendItem()
        {
            LegendItem item = CreateLegendItemCore();
            return RaiseResolveLegendItem(item);
        }

        protected virtual LegendItem CreateLegendItemCore()
        {
            return null;
        }

        public event EventHandler<ResolveLegendItemEventArgs> ResolveLegendItem;
        private LegendItem RaiseResolveLegendItem(LegendItem uncustomizedLegendItem)
        {
            if (ResolveLegendItem != null)
            {
                ResolveLegendItemEventArgs e = new ResolveLegendItemEventArgs(uncustomizedLegendItem);
                ResolveLegendItem(this, e);
                return e.LegendItem;
            }
            else
            {
                return uncustomizedLegendItem;
            }
        }

        private UIElement viewportElement;
        public UIElement ViewportElement
        {
            get { return viewportElement; }
        }

        internal void Attach(UIElement element)
        {
            this.viewportElement = element;
            AttachCore(element);
        }

        protected virtual void AttachCore(UIElement element) { }

        internal void Detach()
        {
            viewportElement = null;
        }

        public abstract string Brief { get; }

        public abstract string Full { get; }

        public override string ToString()
        {
            return Brief;
        }
    }


    public class StandardDescription : Description
    {
        public StandardDescription() { }
        public StandardDescription(string description)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");

            this.description = description;
        }

        protected override void AttachCore(UIElement element)
        {
            if (description == null)
            {
                string str = element.GetType().Name;
                description = str;
            }
        }

        private string description;
        public string DescriptionString
        {
            get { return description; }
            set { description = value; }
        }

        public sealed override string Brief
        {
            get { return description; }
        }

        public sealed override string Full
        {
            get { return description; }
        }
    }


    public sealed class PenDescription : StandardDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PenDescription"/> class.
        /// </summary>
        public PenDescription() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PenDescription"/> class.
        /// </summary>
        /// <param name="description">Custom description.</param>
        public PenDescription(string description) : base(description) { }

        //protected override LegendItem CreateLegendItemCore()
        //{
        //    return new LineLegendItem(this);
        //}

        protected override void AttachCore(UIElement graph)
        {
            base.AttachCore(graph);

            //LineGraph g = graph as LineGraph;

            //if (g == null)
            //{
            //    throw new ArgumentException("Pen description can only be attached to PointsGraph", "graph");
            //}
            //pen = g.LinePen;

            pen = new Pen(Brushes.Red, 1);
        }

        private Pen pen;

        public Pen Pen
        {
            get { return pen; }
        }
    }
    #endregion


}

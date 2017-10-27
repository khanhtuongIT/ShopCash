using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;

namespace CashierRegister.Assets
{
    class Link : NotifyPropertyChanged
    {
        private System.Windows.Media.ImageSource imageSource;

        /// <summary>
        /// Gets or sets the source image.
        /// </summary>
        /// <value>The source.</value>
        public System.Windows.Media.ImageSource ImageSource
        {
            get { return this.imageSource; }
            set
            {
                if (this.imageSource != value)
                {
                    this.imageSource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }
    }
}

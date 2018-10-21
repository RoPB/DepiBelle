using System;
using System.Runtime.CompilerServices;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;

namespace DepiBelleDepi.Extensions
{
    public class SvgImage : SvgCachedImage
    {
        private static string assemblyName;
        private const string ResourcePrefix = "resource://";
        private const string ResourcePath = "Assets.Images";
        private const string SvgExtension = ".svg";

        private static string AssemblyName
        {
            get
            {
                if (assemblyName == null)
                {
                    assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                }
                return assemblyName;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(this.Source))
            {
                if (this.Source is FileImageSource && !((FileImageSource)this.Source).File.Contains(ResourcePrefix))
                {
                    this.Source = $"{ResourcePrefix}{AssemblyName}.{ResourcePath}.{((FileImageSource)this.Source).File}{SvgExtension}";
                }
            }
        }
    }
}

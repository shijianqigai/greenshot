﻿#region Greenshot GNU General Public License

// Greenshot - a free and open source screenshot tool
// Copyright (C) 2007-2018 Thomas Braun, Jens Klingen, Robin Krom
// 
// For more information see: http://getgreenshot.org/
// The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 1 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Diagnostics;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Toasts.ViewModels;
using Dapplo.Log;
using Dapplo.Windows.Extensions;
using Greenshot.Addons.Components;
using Greenshot.Addons.Core;
using Greenshot.Addons.Interfaces;

namespace Greenshot.Addons.ViewModels
{
    /// <inheritdoc />
    public class ExportNotificationViewModel : ToastBaseViewModel
    {
        private static readonly LogSource Log = new LogSource();
        public ExportNotificationViewModel(IDestination source, ExportInformation exportInformation, ISurface exportedSurface)
        {
            Information = exportInformation;
            Source = source;

            using (var bitmap = exportedSurface.GetBitmapForExport())
            {
                ExportBitmapSource = bitmap.ToBitmapSource();
            }
        }

        public ImageSource GreenshotIcon => GreenshotResources.GreenshotIconAsBitmapSource();

        public ImageSource ExportBitmapSource { get; }

        public IDestination Source { get; }

        public ExportInformation Information { get; }

        /// <summary>
        /// Handle the click
        /// </summary>
        public void OpenExport()
        {
            try
            {
                if (Information.IsFileExport)
                {
                    ExplorerHelper.OpenInExplorer(Information.Filepath);
                    return;
                }

                if (Information.IsCloudExport)
                {
                    Process.Start(Information.Uri);
                }
            }
            catch (Exception ex)
            {
                Log.Error().WriteLine(ex,"While opening {0}", Information.Uri);
            }
            finally
            {
                Close();
            }
        }
    }
}

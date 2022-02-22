/*
    Copyright (C) 2014-2019 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Windows.Input;
using dnSpy.Contracts.Debugger;
using dnSpy.Contracts.Debugger.DotNet.Mono;
using dnSpy.Contracts.Debugger.StartDebugging.Dialog;
using dnSpy.Contracts.MVVM;
using dnSpy.Debugger.DotNet.Mono.Properties;

namespace dnSpy.Debugger.DotNet.Mono.Dialogs.DebugProgram {
	sealed class UnityConnectAndroidStartDebuggingOptionsPage : MonoConnectStartDebuggingOptionsPageBase {
		public override Guid Guid => new Guid("D244A779-9A4B-401B-99D4-6546B313260B");
		public override double DisplayOrder => PredefinedStartDebuggingOptionsPageDisplayOrders.DotNetUnityAndroidConnect;
		// Shouldn't be localized
		public override string DisplayName => "Unity (" + dnSpy_Debugger_DotNet_Mono_Resources.DbgAsm_Connect_To_Process + " - Android)";

		// Default port used by the patched mono.dll
		const ushort DEFAULT_PORT = 55555;

		const string DEFAULT_IPADDRESS = "127.0.0.1";
		public ICommand PickWorkingDirectoryCommand => new RelayCommand(a => PickNewWorkingDirectory());

		public ICommand DebuggingUnityGamesCommand => new RelayCommand(a => DebuggingUnityGamesHelper.OpenDebuggingUnityGames());
		public string DebuggingUnityGamesText => DebuggingUnityGamesHelper.DebuggingUnityGamesText;

		public string? WorkingDirectory {
			get => workingDirectory;
			set {
				if (workingDirectory != value) {
					workingDirectory = value;
					OnPropertyChanged(nameof(WorkingDirectory));
					//UpdateIsValid();
				}
			}
		}
		string? workingDirectory = string.Empty;

		readonly IPickDirectory pickDirectory = new PickDirectory();

		void PickNewWorkingDirectory() {
			var newDir = pickDirectory.GetDirectory(WorkingDirectory);
			if (newDir is null)
				return;

			WorkingDirectory = newDir;
		}

		public override void InitializePreviousOptions(StartDebuggingOptions options) {
			var dncOptions = options as UnityConnectAndroidStartDebuggingOptions;
			if (dncOptions is null)
				return;
			Initialize(dncOptions);
		}

		public override void InitializeDefaultOptions(string filename, string breakKind, StartDebuggingOptions? options) =>
			Initialize(GetDefaultOptions(filename, breakKind, options));

		UnityConnectAndroidStartDebuggingOptions GetDefaultOptions(string filename, string breakKind, StartDebuggingOptions? options) {
			if (options is UnityConnectAndroidStartDebuggingOptions connectOptions)
				return connectOptions;
			return CreateOptions(breakKind);
		}

		UnityConnectAndroidStartDebuggingOptions CreateOptions(string breakKind) {
			var options = InitializeDefault(new UnityConnectAndroidStartDebuggingOptions(), breakKind);
			options.Port = DEFAULT_PORT;
			options.Address = DEFAULT_IPADDRESS;
			return options;
		}

		void Initialize(UnityConnectAndroidStartDebuggingOptions options) {
			base.Initialize(options);
			workingDirectory = options.WorkingDirectory;
		}

		public override StartDebuggingOptionsInfo GetOptions() {
			var options = GetOptions(new UnityConnectAndroidStartDebuggingOptions());
			options.WorkingDirectory = workingDirectory;
			return new StartDebuggingOptionsInfo(options, null, StartDebuggingOptionsInfoFlags.None);
		}
	}
}

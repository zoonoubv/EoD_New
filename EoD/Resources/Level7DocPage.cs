using System;
using System.IO;
using System.Text.RegularExpressions;
using Gtk;

using NetOffice;
using Word = NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using System.Reflection;

using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;

using System.Security.Permissions;
using Microsoft.Win32;

public partial class MainWindow: Gtk.Window{
	Word.Application wordApplication = null;
	Word.Document newDocument = null;

	String bobone = null;

	public string GetTempPath(){
		string path = System.Environment.GetEnvironmentVariable("TEMP");
		if (!path.EndsWith("\\")) path += "\\";

		return path;
	}

	public void LogMessageToFile(string msg)
	{
		System.IO.StreamWriter sw = System.IO.File.AppendText(
			GetTempPath() + "EoD_Log_File.txt");
		try
		{
			string logLine = System.String.Format(
				"{0:G}: {1}.", System.DateTime.Now, msg);
			sw.WriteLine(logLine);
		}
		finally
		{
			sw.Close();
		}
	}
		
	public void checkWord(){
		LogMessageToFile("Checking Word Version...");
		RegistryKey localMachine = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Office\");

		string version = "Empty";
		string versionLat = "Empty";
		version = versionLat;
		bool emTest = false;
		int iversionN = 0;


		foreach(string key in localMachine.GetSubKeyNames()){
			if (key == "7.0"){
				//version = "1995";
				if( 1 > iversionN)
					iversionN = 1;
			}
			else if (key == "8.0"){
				//version = "1997";
				if( 2 > iversionN)
				iversionN = 2;
			}
			else if (key == "9.0"){
				//version = "2000";
				if( 3 > iversionN)
				iversionN = 3;
			}
			else if (key == "10.0"){
				//version = "XP";
				if( 4 > iversionN)
				iversionN = 4;
			}
			else if (key == "11.0"){
				//version = "2003";
				if( 5 > iversionN)
				iversionN = 5;
			}
			else if (key == "12.0"){
				//version = "2007";
				if( 6 > iversionN)
				iversionN = 6;
			}
			else if (key == "14.0"){
				version = "2010";
				versionLat = "2010";
				emTest = true;
			}
			else if (key == "15.0"){
				version = "2013";
				versionLat = "2013";
				emTest = true;
			}
			else{
				break;
			}
		}

		LogMessageToFile(("Version - " + version + "/" + versionLat));
			
		if(emTest){
			tempbb = true;
		}
		else if (iversionN == 0){
			MessageDialog PF = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, ("Word version not detected! The application needs Microsoft Word (2010/2013) installed"));
			PF.Title= "Microsoft Word Not Installed";
			ResponseType response = (ResponseType) PF.Run();
			if (response == ResponseType.Ok || response == ResponseType.DeleteEvent){
				PF.Destroy();
			}
		}
		else{
			switch(iversionN){
			case 0:
				version = "Undetected version";
				break;
			case 1:
				version = "1995";
				break;
			case 2:
				version = "1997";
				break;
			case 3:
				version = "2000";
				break;
			case 4:
				version = "XP";
				break;
			case 5:
				version = "2003";
				break;
			case 6:
				version = "2007";
				break;
			default:
				version = "New version";
				break;
			}

			string sMessage;
			bool bError = false;
			if(version == "New version")
				sMessage = ("New version of Word detected. This application supports Microsoft Word (2010/2013). Do you wish to continue? (Please note issues may arise)");
			else if(version == "Undetected version"){
				sMessage = ("Undetected version - Please report this issue to the tool admin.");
				bError = true;
			}
			else
				sMessage = ("Older version of Word detected: " + version + " This application supports Microsoft Word (2010/2013). Do you wish to continue? (Please note issues may arise)");

			if(!bError){
				MessageDialog PF = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.YesNo, sMessage);
				PF.Title= "Unsupported Word Version Detected";
				ResponseType response = (ResponseType) PF.Run();
				if (response == ResponseType.No || response == ResponseType.DeleteEvent){
					PF.Destroy();
				}else if(response == ResponseType.Yes){
					tempbb = true;
					PF.Destroy();
				}
			}else{
				MessageDialog PF = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.Close, sMessage);
				PF.Title= "Error";
				ResponseType response = (ResponseType) PF.Run();
				if (response == ResponseType.Close || response == ResponseType.DeleteEvent){
					bError = false;
					PF.Destroy();
				}
			}
		}
	}

	public void ReportSectionSeven(){
		SHLevel7();
		MainLabelTitle.Text = "Daily Report Creation";
		
		GtkScrolledWindow.SetPolicy(PolicyType.Never,PolicyType.Never);
		M1H1MainLabelHeader1.WidthRequest = 700;
		M1H1MainLabelHeader1.Text = ("                                                                Select a file location then click 'Create & Close'.");

		button8.Label = "Set File Location.";
		label10.Text = "Path: ";

		if(mytempfilename == ""){
			M1MainTextView1.Buffer.Text = "No path selected.";
			MainButtonControls1.Sensitive = false;
		}else{
			M1MainTextView1.Buffer.Text = mytempfilename;
			MainButtonControls1.Sensitive = true;
		}

		M1MainTextView1.Sensitive = true;
		M1MainTextView1.Editable = false;
		M1MainTextView1.WrapMode = WrapMode.Char;
		M1MainTextView1.HeightRequest = 50;
		GtkScrolledWindow.SetPolicy(PolicyType.Never,PolicyType.Never);

		button8.WidthRequest = 150;
		button8.Sensitive = true;
		MainButtonControls1.Label = @"Create & Close";

		//open report setting
		M1H2MainCheck1.Label = "Open report after creation";
		M1H2MainCheck1.Active = true;
	}

	public void SHLevel7(){
		MainVboxSubContainerM1.HideAll();
		MainVboxSubContainerM1.Show();
		MainHboxSubContainerM1H1.Show();
		M1H1MainLabelHeader1.Show();
		MainHboxSubContainerM1H2.Show();

		M1H2MainCheck1.ShowAll();

		hbox8.Show();
		GtkScrolledWindow.ShowAll();
		M1MainTextView1.Show();

		hbox9.Show();
		button8.Show();
		hbox11.Show();
		label10.Show();

		//Hide other sections
		MainVboxSubContainerM2.Hide();
		MainVboxSubContainerM3.Hide();
		MainVboxSubContainerM4.Hide();
		MainVboxSubContainerM5.Hide();
	}

	#region doc Start

	public void CreateDoc(){
		LogMessageToFile(@"////////////// CREATE \\\\\\\\\\\\\\\");
		LogMessageToFile("Starting application...");
		LogMessageToFile("Start a new word process...");

		wordApplication = new Word.Application();
		bobone = wordApplication.Version;

		LogMessageToFile("Remove alerts...");
		wordApplication.DisplayAlerts = WdAlertLevel.wdAlertsNone;
		LogMessageToFile("Add a new document to the active word session...");
		newDocument = wordApplication.Documents.Add();

		LogMessageToFile("Start Main creation...");
		startDoc();
	}

	public void restartDoc(){
		LogMessageToFile("Restart creation / word session");
		wordApplication = new Word.Application();
		newDocument = wordApplication.Documents.Add();
		startDoc();
	}

	public void startDoc (){
		LogMessageToFile("Set font and size");
		wordApplication.Selection.Font.Size = 11;
		wordApplication.Selection.Font.Name = "Corbel";

		LogMessageToFile("Start table creation");

		//log
		string contentText = "--User Entered details---" + "\r\n"
			+ "---File: \r\n" 
			+ mytempfilename + "\r\n"
			+ "---Client name: \r\n"
			+ clientNameString + "\r\n"
			+ "---Project: \n"
			+ projectNameString + "\r\n"
			+ "---Tester(s): " + "\r\n"
			+ sAllinitials + "\r\n"
			+ "---Date: " + "\r\n"
			+ sDateTested + "\r\n"
			+ "---URL(s) tested:" + "\r\n"
			+ urlUsedString + "\r\n"
			+ "---Build version(s) tested:" + "\r\n"
			+ buildVersionString + "\r\n"
			+ "---Report Details---" + "\r\n"
			+ sTTC + "\r\n"
			+ "----Main report----" + "\r\n"
			+ sBOOT + "\r\n"
			+ "---Blocking---" + "\r\n"
			+ sBlockingyN + "\r\n"
			+ "---Blocking Issues:"  + "\r\n"
			+ sBlockingNumbers + "\r\n"
			+ "---Top5---" + "\r\n"
			+ top5ListArray[0] + "\r\n"
			+ top5ListArray[1] + "\r\n"
			+ top5ListArray[2] + "\r\n"
			+ top5ListArray[3] + "\r\n"
			+ top5ListArray[4] + "\r\n"
			+ "---Mecs---" + "\r\n"
			+ sMetric1 + "\r\n"
			+ sMetric2 + "\r\n"
			+ sMetric3 + "\r\n"
			+ sMetric4;

		LogMessageToFile(contentText);

		firstTable();
		moveDownpar();
		thirdTable();
		moveDownpar();
		fouthTable();
		moveDownpar();
		fithTable();
		moveDownpar();

		LogMessageToFile("Adjust various page settings...");
		wordApplication.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageHeader;
		string sImageLoc = Environment.CurrentDirectory + @"\Resources\ZoonouLogo.jpg";
		wordApplication.Selection.InlineShapes.AddPicture(sImageLoc); 
		wordApplication.Selection.MoveRight();
		wordApplication.Selection.PageSetup.HeaderDistance = 12.00f;
		wordApplication.Selection.PageSetup.FooterDistance = 6.00f;
		wordApplication.Selection.PageSetup.LeftMargin = 72;
		wordApplication.Selection.PageSetup.RightMargin = 72;
		wordApplication.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;
		wordApplication.Selection.WholeStory();

		if(bobone == "14.0"){
			wordApplication.Selection.ParagraphFormat.LeftIndent = 4.50f;
		}
		else{
			wordApplication.Selection.ParagraphFormat.LeftIndent = 0;
		}

		wordApplication.Selection.Font.Color = WdColor.wdColorBlack;
		string documentFile = null;

		if(Directory.Exists(mytempfilename)){

			string sDateTemp = sDateTested;
			string invalid = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());

			foreach (char c in invalid){
				sDateTemp = sDateTemp.Replace(c.ToString(), ""); 
			}

			documentFile = (mytempfilename + @"\" + clientNameString + " - " + projectNameString + " - Daily Report " + sDateTemp +  @".docx");
			bool mynewloop = false;
			int myloopint= 0;

			LogMessageToFile("Detect if file exists already...");
			do{
				if(File.Exists(documentFile)){
					documentFile = (mytempfilename + @"\" + clientNameString + " - " + projectNameString + " - Daily Report " + sDateTemp + " (v" + (++myloopint) + ")" + @".docx");
					LogMessageToFile("File existed - adding number to file name...");
				}else{
					mynewloop = true;
				}
			}while(!mynewloop);

			double wordVersion = Convert.ToDouble(wordApplication.Version, CultureInfo.InvariantCulture);

			LogMessageToFile("Save file...");
			if (wordVersion >= 12.0){
				newDocument.SaveAs(documentFile, WdSaveFormat.wdFormatDocumentDefault);
			}
			else{
				newDocument.SaveAs(documentFile);
			}
				
			object saveOption = WdSaveOptions.wdDoNotSaveChanges;
			object orginalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
			object routeDocument = false;

			LogMessageToFile("Release resources...");

			newDocument.Close(saveOption, orginalFormat, routeDocument);
			wordApplication.Quit();
			wordApplication.Dispose();

			if(M1H2MainCheck1.Active){
				LogMessageToFile("Open Doc...");
				Process.Start(documentFile);
			}

			LogMessageToFile("---Finish---");
			Application.Quit();
		}
		else{
			MessageDialog E1 = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, "Folder path removed - Please retry.");
			E1.WidthRequest = 600; 
			E1.Title= "Folder Path Removed";

			ResponseType response = (ResponseType) E1.Run();

			if (response == ResponseType.Ok || response == ResponseType.DeleteEvent){
				E1.Destroy();
			}

			object saveOption = WdSaveOptions.wdDoNotSaveChanges;
			object orginalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
			object routeDocument = false;
			newDocument.Close(saveOption, orginalFormat, routeDocument);
			wordApplication.Quit();
			wordApplication.Dispose();

			bDocRan = true;

			FilePickerClicked();
		}
	}

	protected void FilePickerClicked(){
		Gtk.FileChooserDialog fc = new Gtk.FileChooserDialog("Choose a folder path", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Choose", ResponseType.Accept);
		mytempfilename = "";
		bool btemp1 = false;

		do{
			if(fc.Run() == (int)ResponseType.Accept){
				mytempfilename = fc.CurrentFolder;

				if(mytempfilename != null){
					MainVboxSubContainerM2.HideAll();
					MainVboxSubContainerM2.Show();
					MainHboxSubContainerM2H1.Show();
					M2H1MainLabelHeader1.Show();
					M2H1MainLabelHeader1.WidthRequest = 500;
					M2H1MainLabelHeader1.Justify = Justification.Center;

					M2H1MainLabelHeader1.Text = (@"
					
                                                   When you click 'Create & Close' the daily report will be created.

                                                    This application will close once completed.");

					M1MainTextView1.Buffer.Text = mytempfilename;

					fc.Destroy();
					MainButtonControls1.Sensitive = true;
					btemp1 = true;
				}
			}else{
				btemp1 = true;
				fc.Destroy();
			}
		}while(!btemp1);
	}

	#region Tables

	public void moveDownpar(){
		wordApplication.ActiveDocument.Characters.Last.Select();
		wordApplication.Selection.Collapse();

		//wordApplication.Selection.MoveDown();
		//wordApplication.Selection.MoveDown();
		wordApplication.Selection.TypeParagraph();
		return;
	}

	public void firstTable(){
		Word.Table table = newDocument.Tables.Add(wordApplication.Selection.Range, 8, 2);

		table.Columns[1].SetWidth(160.00f, WdRulerStyle.wdAdjustFirstColumn); 
		table.Columns[2].SetWidth(291.50f, WdRulerStyle.wdAdjustSameWidth); 

		table.Cell(1,1).Select();
		wordApplication.Selection.Font.Bold = 1;
		wordApplication.Selection.TypeText(@"Project Details");
		table.Cell(2,1).Select();
		wordApplication.Selection.TypeText(@"Client:");
		table.Cell(3,1).Select();
		wordApplication.Selection.TypeText(@"Project:");
		table.Cell(4,1).Select();
		wordApplication.Selection.TypeText(@"URL(s) tested:");
		table.Cell(5,1).Select();
		wordApplication.Selection.TypeText(@"Build version(s) tested:");


		table.Cell(6,1).Select();
		wordApplication.Selection.TypeText(@"Tester(s):");
		table.Cell(7,1).Select();
		wordApplication.Selection.TypeText(@"Date:");

		table.Cell(6,2).Select();
		wordApplication.Selection.TypeText(sAllinitials);
		table.Cell(7,2).Select();
		wordApplication.Selection.TypeText(sDateTested);


		table.Cell(8,1).Select();
		wordApplication.Selection.TypeText(@"Test environment(s):");

		table.Cell(2,2).Select();
		wordApplication.Selection.TypeText(clientNameString);
		table.Cell(3,2).Select();
		wordApplication.Selection.TypeText(projectNameString);
		table.Cell(4,2).Select();
		wordApplication.Selection.TypeText(urlUsedString);
		table.Cell(5,2).Select();
		wordApplication.Selection.TypeText(buildVersionString);
		table.Cell(8,2).Select();

		string sTemp = "";

		if(primEnabled){
			if(primListArray.Length >1)
				sTemp = "Primary environments:\n";
			else
				sTemp = "Primary environment:\n";

			if(primListArray.Length == 1){
				sTemp += (primListArray[0] + "\n");
			}

			if(primListArray.Length > 1){
				for(int x = 0; x < primListArray.Length; x++){
					sTemp += (primListArray[x] + "\n");
				}
			}
		}

		if(bSmokes){
			if(primEnabled){
				sTemp += "\n";
			}
			sTemp += ("For cross environment checks/smoke tests, please see the environments detailed in the table at the end of the report.\n");
		}

		if(bIssueVoption){
			if(primEnabled){
				sTemp += "\n";
			}

			if((!primEnabled)&&(bSmokes)){
				sTemp += "\n";
			}
				
			sTemp += ("Retests executed in environments the issues were originally raised in.\n\n");
			//sTemp += ("Additional regression testing carried out in the time remaining.\n");
		}

	
		wordApplication.Selection.TypeText(sTemp);
		wordApplication.Selection.TypeBackspace();

		tableVersion(ref table,8);

		table.Dispose();
		return;
	}

	public void tableVersion(ref Word.Table table, int rows){
		if(bobone == "14.0"){
			table.Style = "Light Shading - Accent 1";
			table.ApplyStyleFirstColumn = false;
			table.ApplyStyleHeadingRows = false;
			//table.Rows.SetLeftIndent(4.50f, WdRulerStyle.wdAdjustNone);

			WdColor test = (WdColor)16181982;

			for(int n = 1; n <= rows; n += 2)
			{
				table.Rows[n].Shading.BackgroundPatternColor = test;
			}
		}
		else if(bobone == "15.0"){
			table.Style = "List Table 6 Colorful - Accent 1";
			table.ApplyStyleFirstColumn = false;
			table.ApplyStyleHeadingRows = false;
		}
		else{
			table.Style = "List Table 6 Colorful - Accent 1";
			table.ApplyStyleFirstColumn = false;
			table.ApplyStyleHeadingRows = false;
		}
		return;
	}

	public void secondTable(){
		Word.Table table = newDocument.Tables.Add(wordApplication.Selection.Range, 3, 2);

		table.Columns[1].SetWidth(160.00f, WdRulerStyle.wdAdjustFirstColumn); 
		table.Columns[2].SetWidth(291.50f, WdRulerStyle.wdAdjustSameWidth); 

		table.Cell(1,1).Select();
		wordApplication.Selection.Font.Bold = 1;
		wordApplication.Selection.TypeText(@"Report Details");
		table.Cell(2,1).Select();
		wordApplication.Selection.TypeText(@"Tester Name:");
		table.Cell(3,1).Select();
		wordApplication.Selection.TypeText(@"Date:");

		table.Cell(2,2).Select();
		wordApplication.Selection.TypeText(sAllinitials);
		table.Cell(3,2).Select();
		wordApplication.Selection.TypeText(sDateTested);

		tableVersion(ref table,3);

		table.Dispose();
		return;
	}

	public void thirdTable(){
		Word.Table table = newDocument.Tables.Add(wordApplication.Selection.Range, 4, 2);

		table.Columns[1].SetWidth(160.00f, WdRulerStyle.wdAdjustFirstColumn); 
		table.Columns[2].SetWidth(291.50f, WdRulerStyle.wdAdjustSameWidth);  

		table.Cell(1,1).Select();
		wordApplication.Selection.Font.Bold = 1;
		wordApplication.Selection.TypeText(@"Report Details");
		table.Cell(2,1).Select();
		wordApplication.Selection.TypeText(@"Test activities:");
		table.Cell(3,1).Select();
		wordApplication.Selection.TypeText(@"Test tasks completed:");
		table.Cell(4,1).Select();
		wordApplication.Selection.TypeText(@"Brief overview of testing:");

		string activtemp = "";

		if(bScripting){
			activtemp = @"Scripting & Planning ";
			if(bTestExe){
				activtemp = activtemp + @"/ Test Execution ";
			}
			if(bIssueVoption){
				activtemp = activtemp + @"/ Issue Verification & Retests ";
			}
		}else if(bTestExe){
			activtemp = activtemp + @"Test Execution ";
			if(bIssueVoption){
				activtemp = activtemp + @"/ Issue Verification & Retests ";
			}
		}else if(bIssueVoption){
			activtemp = activtemp + @"Issue Verification & Retests ";
		}

		table.Cell(2,2).Select();
		wordApplication.Selection.TypeText(activtemp);
		table.Cell(3,2).Select();
		wordApplication.Selection.TypeText(sTTC);
		table.Cell(4,2).Select();
		wordApplication.Selection.TypeText(sBOOT);
	
		tableVersion(ref table,4);

		table.Dispose();
		return;
	}

	public void fouthTable(){
		Word.Table table = newDocument.Tables.Add(wordApplication.Selection.Range, 9, 2);

		table.Columns[1].SetWidth(160.00f, WdRulerStyle.wdAdjustFirstColumn); 
		table.Columns[2].SetWidth(291.50f, WdRulerStyle.wdAdjustSameWidth); 

		table.Cell(1,1).Select();
		wordApplication.Selection.Font.Bold = 1;
		wordApplication.Selection.TypeText(@"Issue Summary");
		table.Cell(2,1).Select();
		wordApplication.Selection.TypeText(@"Have any issues been found that block test progress?");
		table.Cell(3,1).Select();
		wordApplication.Selection.TypeText(@"Blocking Issue(s) found:");
		table.Cell(4,1).Select();
		wordApplication.Selection.TypeText(@"Top 5 issues of concern:");

		table.Cell(5,1).Select();
		wordApplication.Selection.TypeText(@"1:");
		wordApplication.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
		table.Cell(6,1).Select();
		wordApplication.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
		wordApplication.Selection.TypeText(@"2:");
		table.Cell(7,1).Select();
		wordApplication.Selection.TypeText(@"3:");
		wordApplication.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
		table.Cell(8,1).Select();
		wordApplication.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
		wordApplication.Selection.TypeText(@"4:");
		table.Cell(9,1).Select();
		wordApplication.Selection.TypeText(@"5:");
		wordApplication.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

		table.Cell(2,2).Select();
		wordApplication.Selection.TypeText(sBlockingyN);
		table.Cell(3,2).Select();
		wordApplication.Selection.TypeText(sBlockingNumbers);
		table.Cell(5,2).Select();
		wordApplication.Selection.TypeText(top5ListArray[0]);
		table.Cell(6,2).Select();
		wordApplication.Selection.TypeText(top5ListArray[1]);
		table.Cell(7,2).Select();
		wordApplication.Selection.TypeText(top5ListArray[2]);
		table.Cell(8,2).Select();
		wordApplication.Selection.TypeText(top5ListArray[3]);
		table.Cell(9,2).Select();
		wordApplication.Selection.TypeText(top5ListArray[4]);

		tableVersion(ref table,9);

		table.Dispose();
		return;
	}

	public void fithTable(){
		Word.Table table = newDocument.Tables.Add(wordApplication.Selection.Range, 5, 2);

		table.Columns[1].SetWidth(160.00f, WdRulerStyle.wdAdjustFirstColumn); 
		table.Columns[2].SetWidth(291.50f, WdRulerStyle.wdAdjustSameWidth); 

		table.Cell(1,1).Select();
		wordApplication.Selection.Font.Bold = 1;
		wordApplication.Selection.TypeText(@"Metrics");
		table.Cell(2,1).Select();
		wordApplication.Selection.TypeText(@"New issues raised today:");
		table.Cell(3,1).Select();
		wordApplication.Selection.TypeText(@"Issues re-opened today:");
		table.Cell(4,1).Select();
		wordApplication.Selection.TypeText(@"Issues closed today:");
		table.Cell(5,1).Select();
		wordApplication.Selection.TypeText(@"Total number of open issues:");

		table.Cell(2,2).Select();
		wordApplication.Selection.TypeText(sMetric1);
		table.Cell(3,2).Select();
		wordApplication.Selection.TypeText(sMetric2);
		table.Cell(4,2).Select();
		wordApplication.Selection.TypeText(sMetric3);
		table.Cell(5,2).Select();
		wordApplication.Selection.TypeText(sMetric4);

		tableVersion(ref table,5);

		table.Dispose();
		return;
	}

	#endregion

	#endregion
}


using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace InstagramBot
{
	public class MainWindow : Window, IComponentConnector
	{
		private InstaBot Bot;

		private int TrialStatus;

		internal Grid BotWindow;

		internal TabControl tabControl;

		internal TabItem contentTab;

		internal GroupBox groupBoxLogin;

		internal TextBox loginBox;

		internal PasswordBox passwordBox;

		internal Label label1;

		internal Label label2;

		internal GroupBox groupBoxAfterLogin;

		internal Image photoImg_You;

		internal Label label3;

		internal Label label3_Copy;

		internal Label label3_Copy1;

		internal Label PFollowingC;

		internal Label PFollowerC;

		internal Label PMediaC;

		internal Label label;

		internal GroupBox groupBox1;

		internal Label label4;

		internal Label label6;

		internal Label label7;

		internal Label label8;

		internal IntegerUpDown likesPerDayBox;

		internal IntegerUpDown followPerDayBox;

		internal IntegerUpDown unfollowPerDayBox;

		internal IntegerUpDown commentsPerDayBox;

		internal Label label4_Copy;

		internal Label label6_Copy;

		internal Label label7_Copy;

		internal Label label8_Copy;

		internal Label NextLikeTimeBox;

		internal Label NextFollowTimeBox;

		internal Label NextUnfollowTimeBox;

		internal Label NextCommentTimeBox;

		internal TextBox logBox;

		internal Label label5;

		internal Button startButton;

		internal Button stopButton;

		internal Rectangle workAnimation;

		internal RotateTransform rotateThis;

		internal Button logoutButton;

		internal Button loginButton;

		internal ProgressBar pbLoading;

		internal GroupBox mediaInfo;

		internal Label label13;

		internal Label label12;

		internal Label labelInBotCommentsC;

		internal Label label10;

		internal Image photoImg;

		internal Label labelInBotMediaId;

		internal Label label11;

		internal Label labelInBotLikesC;

		internal Label label14;

		internal Run labelInBotUserT;

		internal Run labelInBotMediaUrlT;

		internal TabItem tabSettings;

		internal GroupBox botAddSettings;

		internal Label label9;

		internal GroupBox groupBox6;

		internal Label label21;

		internal TextBox minLikesTB;

		internal Label label22;

		internal TextBox maxLikesTB;

		internal Label label23;

		internal GroupBox groupBox3;

		internal CheckBox SaveFollowList;

		internal GroupBox groupBox2;

		internal Label label19;

		internal TextBox FollowTime;

		internal CheckBox UnfollowOnlyCB;

		internal CheckBox DontUnfollowUserFollowYou;

		internal GroupBox groupBox4;

		internal Button CommentExample;

		internal GroupBox groupBox5;

		internal Label label20;

		internal TextBox maxLineLogBox;

		internal GroupBox groupBox7;

		internal TextBox tagBox;

		internal GroupBox groupBox7_Copy;

		internal TextBox stopBox;

		internal GroupBox apiSettings;

		internal Label label16;

		internal Label label15;

		internal TextBox igSigKeyBox;

		internal TextBox igSigKeyVersion;

		internal Label label15_Copy;

		internal TextBox igSigUserAgent;

		internal TabItem tabAbout;

		internal TextBox aboutBox;

		internal Label label17;

		internal TextBox trialBoxText;

		internal Label label18;

		internal TextBlock trialVText;

		internal Run onGitHub;

		internal Run sendEmail;

		private bool _contentLoaded;

		public MainWindow()
		{
			CallBackLog.callbackEventHandler = WriteLog;
			CallBackMedia.callbackEventHandler = ShowBotMedia;
			CallBackTime.callbackEventHandler = ShowTime;
			InitializeComponent();
		}

		private void WriteLog(string log)
		{
			TextBox textBox = logBox;
			textBox.Text = textBox.Text + log + Environment.NewLine;
		}

		private void SaveSettings()
		{
			string contents = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"login",
					loginBox.Text
				},
				{
					"tagBox",
					tagBox.Text
				},
				{
					"stopBox",
					stopBox.Text
				},
				{
					"likesPerDayBox",
					((InputBase)likesPerDayBox).get_Text()
				},
				{
					"followPerDayBox",
					((InputBase)followPerDayBox).get_Text()
				},
				{
					"unfollowPerDayBox",
					((InputBase)unfollowPerDayBox).get_Text()
				},
				{
					"commentsPerDayBox",
					((InputBase)commentsPerDayBox).get_Text()
				},
				{
					"igSigKeyBox",
					igSigKeyBox.Text
				},
				{
					"igSigKeyVersion",
					igSigKeyVersion.Text
				},
				{
					"trialBoxText",
					trialBoxText.Text
				},
				{
					"igSigUserAgent",
					igSigUserAgent.Text
				},
				{
					"SaveFollowList",
					SaveFollowList.IsChecked.Value ? "1" : "0"
				},
				{
					"FollowTime",
					FollowTime.Text
				},
				{
					"UnfollowOnlyCB",
					UnfollowOnlyCB.IsChecked.Value ? "1" : "0"
				},
				{
					"maxLineLogBox",
					maxLineLogBox.Text
				},
				{
					"minLikesTB",
					minLikesTB.Text
				},
				{
					"maxLikesTB",
					maxLikesTB.Text
				},
				{
					"photoImg_You.Source",
					Convert.ToString(photoImg_You.Source)
				},
				{
					"groupBoxAfterLogin.Header",
					Convert.ToString(groupBoxAfterLogin.Header)
				},
				{
					"DontUnfollowUserFollowYou",
					DontUnfollowUserFollowYou.IsChecked.Value ? "1" : "0"
				}
			}).ToString();
			new FileInfo(Environment.CurrentDirectory + "\\data\\").Directory.Create();
			File.WriteAllText(Environment.CurrentDirectory + "\\data\\" + loginBox.Text + "-settings.dat", contents);
			File.WriteAllText(Environment.CurrentDirectory + "\\data\\last-login.dat", loginBox.Text);
		}

		private void LoadSettings()
		{
			try
			{
				string text = File.ReadAllText(Environment.CurrentDirectory + "\\data\\last-login.dat");
				loginBox.Text = text;
				dynamic val = JsonConvert.DeserializeObject(File.ReadAllText(Environment.CurrentDirectory + "\\data\\" + loginBox.Text + "-settings.dat"));
				tagBox.Text = val["tagBox"];
				stopBox.Text = val["stopBox"];
				((InputBase)likesPerDayBox).set_Text((string)val["likesPerDayBox"]);
				((InputBase)followPerDayBox).set_Text((string)val["followPerDayBox"]);
				((InputBase)unfollowPerDayBox).set_Text((string)val["unfollowPerDayBox"]);
				((InputBase)commentsPerDayBox).set_Text((string)val["commentsPerDayBox"]);
				igSigKeyBox.Text = val["igSigKeyBox"];
				igSigKeyVersion.Text = val["igSigKeyVersion"];
				trialBoxText.Text = val["trialBoxText"];
				igSigUserAgent.Text = val["igSigUserAgent"];
				FollowTime.Text = val["FollowTime"];
				maxLineLogBox.Text = val["maxLineLogBox"];
				minLikesTB.Text = val["minLikesTB"];
				maxLikesTB.Text = val["maxLikesTB"];
				photoImg_You.Source = new BitmapImage(new Uri("" + val["photoImg_You.Source"]));
				groupBoxAfterLogin.Header = (object)val["groupBoxAfterLogin.Header"];
				SaveFollowList.IsChecked = ((val["SaveFollowList"] == "1") ? true : false);
				UnfollowOnlyCB.IsChecked = ((val["UnfollowOnlyCB"] == "1") ? true : false);
				DontUnfollowUserFollowYou.IsChecked = ((val["DontUnfollowUserFollowYou"] == "1") ? true : false);
				WriteLog("Load saved settings success!");
			}
			catch (Exception ex)
			{
				WriteLog("Can't load saved settings... " + ex.Message);
			}
		}

		private async void loginButton_Click(object sender, RoutedEventArgs e)
		{
			string login = Convert.ToString(loginBox.Text);
			string password = Convert.ToString(passwordBox.Password);
			if (login != "" && password != "")
			{
				Bot = new InstaBot();
				Bot.WriteLog("Try to login as " + login + "...");
				loginButton.IsEnabled = false;
				startWorkAnimation();
				try
				{
					if (File.Exists(Environment.CurrentDirectory + "\\data\\" + login + "-session.dat") && MessageBox.Show("You have saved session, use it?", "Login method", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
					{
						if (await Bot.LoginFromFile(login, password))
						{
							groupBoxLogin.IsEnabled = false;
							startButton.IsEnabled = true;
							CommentExample.IsEnabled = true;
							loginButton.Visibility = Visibility.Hidden;
							logoutButton.Visibility = Visibility.Visible;
							Bot.WriteLog("Open saved session success!");
							groupBoxLogin.Visibility = Visibility.Hidden;
							groupBoxAfterLogin.Visibility = Visibility.Visible;
							SaveSettings();
							return;
						}
						if (MessageBox.Show("Fail to open saved session! Start standart login...?", "Login method", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
						{
							loginButton.IsEnabled = true;
							Bot.WriteLog("Stop by user...");
							return;
						}
					}
				}
				catch (ArgumentNullException)
				{
				}
				if (await Bot.Login(login, password))
				{
					groupBoxLogin.IsEnabled = false;
					startButton.IsEnabled = true;
					CommentExample.IsEnabled = true;
					loginButton.Visibility = Visibility.Hidden;
					logoutButton.Visibility = Visibility.Visible;
					Bot.WriteLog("Login success!");
					groupBoxLogin.Visibility = Visibility.Hidden;
					groupBoxAfterLogin.Visibility = Visibility.Visible;
					SaveSettings();
				}
				else
				{
					loginButton.IsEnabled = true;
					string caption = "Error Detected while login";
					MessageBox.Show("Bot can't login!", caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
			else
			{
				string caption2 = "Error Detected in login data";
				MessageBox.Show("Write your login and password!", caption2);
			}
			stopWorkAnimation();
		}

		private void logoutButton_Click(object sender, RoutedEventArgs e)
		{
			Bot.WriteLog("Logout success!");
			loginButton.Visibility = Visibility.Visible;
			logoutButton.Visibility = Visibility.Hidden;
			groupBoxLogin.IsEnabled = true;
			loginButton.IsEnabled = true;
			startButton.IsEnabled = false;
			groupBoxLogin.Visibility = Visibility.Visible;
			groupBoxAfterLogin.Visibility = Visibility.Hidden;
		}

		private async void startButton_click(object sender, RoutedEventArgs e)
		{
			string[] tag_list = tagBox.Text.Replace(" ", "").Split(',');
			string[] stop_words = stopBox.Text.Replace(" ", "").Split(',');
			Bot.SetSettings(int.Parse(((InputBase)likesPerDayBox).get_Text()), int.Parse(maxLikesTB.Text), int.Parse(minLikesTB.Text), int.Parse(((InputBase)followPerDayBox).get_Text()), int.Parse(FollowTime.Text), int.Parse(((InputBase)unfollowPerDayBox).get_Text()), int.Parse(((InputBase)commentsPerDayBox).get_Text()), tag_list, stop_words, 5, 15, 30, 0, igSigKeyBox.Text, igSigKeyVersion.Text, igSigUserAgent.Text, SaveFollowList.IsChecked.Value, DontUnfollowUserFollowYou.IsChecked.Value);
			startButton.IsEnabled = false;
			stopButton.IsEnabled = true;
			Bot.WriteLog("Start InstaBot.");
			startWorkAnimation();
			disableAll();
			SaveSettings();
			await Bot.AutoMod();
		}

		private void stopButton_click(object sender, RoutedEventArgs e)
		{
			Bot.StopAutoMod();
			enableAll();
			stopWorkAnimation();
			startButton.IsEnabled = true;
			stopButton.IsEnabled = false;
			NextLikeTimeBox.Content = "00:00:00";
			NextFollowTimeBox.Content = "00:00:00";
			NextUnfollowTimeBox.Content = "00:00:00";
			NextCommentTimeBox.Content = "00:00:00";
			Bot.WriteLog("Stop InstaBot.");
		}

		private void stopWorkAnimation()
		{
			pbLoading.Visibility = Visibility.Hidden;
		}

		private void startWorkAnimation()
		{
			pbLoading.Visibility = Visibility.Visible;
		}

		private void disableAll()
		{
			DontUnfollowUserFollowYou.IsEnabled = false;
			groupBoxLogin.IsEnabled = false;
			((UIElement)(object)likesPerDayBox).IsEnabled = false;
			((UIElement)(object)followPerDayBox).IsEnabled = false;
			((UIElement)(object)unfollowPerDayBox).IsEnabled = false;
			((UIElement)(object)commentsPerDayBox).IsEnabled = false;
			tagBox.IsEnabled = false;
			tabSettings.IsEnabled = false;
			logoutButton.IsEnabled = false;
			botAddSettings.IsEnabled = false;
			apiSettings.IsEnabled = false;
			tabAbout.IsEnabled = false;
		}

		private void enableAll()
		{
			((UIElement)(object)likesPerDayBox).IsEnabled = true;
			((UIElement)(object)followPerDayBox).IsEnabled = true;
			((UIElement)(object)unfollowPerDayBox).IsEnabled = true;
			((UIElement)(object)commentsPerDayBox).IsEnabled = ((TrialStatus != 0) ? true : false);
			botAddSettings.IsEnabled = ((TrialStatus != 0) ? true : false);
			tagBox.IsEnabled = true;
			tabSettings.IsEnabled = true;
			logoutButton.IsEnabled = true;
			apiSettings.IsEnabled = true;
			tabAbout.IsEnabled = true;
		}

		private void likesPerDayBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (Convert.ToInt32(e.NewValue) < 0)
			{
				((InputBase)likesPerDayBox).set_Text("0");
			}
			if (TrialStatus == 0 && Convert.ToInt32(e.NewValue) > 500)
			{
				((InputBase)likesPerDayBox).set_Text("500");
			}
		}

		private void followPerDayBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (Convert.ToInt32(e.NewValue) < 0)
			{
				((InputBase)followPerDayBox).set_Text("0");
			}
			if (TrialStatus == 0 && Convert.ToInt32(e.NewValue) > 50)
			{
				((InputBase)followPerDayBox).set_Text("50");
			}
		}

		private void unfollowPerDayBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (Convert.ToInt32(e.NewValue) < 0)
			{
				((InputBase)unfollowPerDayBox).set_Text("0");
			}
			if (TrialStatus == 0 && Convert.ToInt32(e.NewValue) > 50)
			{
				((InputBase)unfollowPerDayBox).set_Text("50");
			}
		}

		private void commentsPerDayBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (Convert.ToInt32(e.NewValue) < 0)
			{
				((InputBase)commentsPerDayBox).set_Text("0");
			}
			if (TrialStatus == 0 && Convert.ToInt32(e.NewValue) > 0)
			{
				((InputBase)commentsPerDayBox).set_Text("0");
			}
		}

		private void tagBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (tagBox.Text == "" || tagBox.Text == " ")
			{
				tagBox.Text = "moto, gear, car, speed, fashion, cute, style";
			}
		}

		private void logBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				List<string> list = logBox.Text.Split(new string[1]
				{
					Environment.NewLine
				}, StringSplitOptions.None).ToList();
				if (list.Count - 1 > Convert.ToInt32(maxLineLogBox.Text))
				{
					list.RemoveAt(0);
					logBox.Text = string.Join(Environment.NewLine, list.ToArray());
				}
			}
			catch
			{
			}
			logBox.ScrollToEnd();
		}

		private void ShowBotMedia(dynamic media, string type)
		{
			try
			{
				if (type == "profile")
				{
					photoImg_You.Source = new BitmapImage(new Uri("" + media.user.profile_pic_url));
					groupBoxAfterLogin.Header = (object)(media.user.username + ":");
					PFollowingC.Content = (object)media.user.following_count;
					PFollowerC.Content = (object)media.user.follower_count;
					PMediaC.Content = (object)media.user.media_count;
					return;
				}
				mediaInfo.Visibility = Visibility.Visible;
				labelInBotCommentsC.Content = (object)media.comment_count;
				labelInBotLikesC.Content = (object)media.like_count;
				if (media.image_versions2.candidates[3].url != null)
				{
					photoImg.Source = new BitmapImage(new Uri("" + media.image_versions2.candidates[3].url));
				}
				labelInBotMediaId.Content = (object)media.pk;
				labelInBotUserT.Text = string.Format("https://www.instagram.com/{0}/", media.user.username);
				labelInBotMediaUrlT.Text = string.Format("https://www.instagram.com/p/{0}/", media.code);
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}

		private void ShowTime(Dictionary<string, int> next)
		{
			int num = Bot.DateNow();
			int num2 = (next["Like"] - num > 0) ? (next["Like"] - num) : 0;
			NextLikeTimeBox.Content = $"{num2 / 3600:00}:{num2 / 60 % 60:00}:{num2 % 60:00}";
			int num3 = (next["Follow"] - num > 0) ? (next["Follow"] - num) : 0;
			NextFollowTimeBox.Content = $"{num3 / 3600:00}:{num3 / 60 % 60:00}:{num3 % 60:00}";
			int num4 = (next["Unfollow"] - num > 0) ? (next["Unfollow"] - num) : 0;
			NextUnfollowTimeBox.Content = $"{num4 / 3600:00}:{num4 / 60 % 60:00}:{num4 % 60:00}";
			int num5 = (next["Comment"] - num > 0) ? (next["Comment"] - num) : 0;
			NextCommentTimeBox.Content = $"{num5 / 3600:00}:{num5 / 60 % 60:00}:{num5 % 60:00}";
		}

		private void OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(labelInBotMediaUrlT.Text);
			}
			catch
			{
			}
		}

		private void OnClick2(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(labelInBotUserT.Text);
			}
			catch
			{
			}
		}

		private void OnClick3(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("https://github.com/LevPasha/Instagram-bot-cs/");
			}
			catch
			{
			}
		}

		private void OnClick4(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("mailto:levpasha@gmail.com");
			}
			catch
			{
			}
		}

		private void WriteAboutText()
		{
			aboutBox.Text = "";
			TextBox textBox = aboutBox;
			textBox.Text = textBox.Text + "Wellcome to C# Instagram Bot!" + Environment.NewLine;
			aboutBox.Text += Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "This bot pretending Insagram application on android OS." + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "Bot been tested with the following settings:" + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "1) Like per day 1000;" + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "2) Follow per day 350;" + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "3) Unfollow per day 350;" + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "4) Comment per day 100;" + Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "If you are going to use more than this limits - we can't guarantee protection against the ban." + Environment.NewLine;
			aboutBox.Text += Environment.NewLine;
			switch (TrialStatus)
			{
			case 0:
				textBox = aboutBox;
				textBox.Text = textBox.Text + "Now you are using a \"Trial\" version with limited settings and function:" + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can like, follow, unfollow with limits 500, 50, 50 per day." + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can't change most of settings." + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can't upload photo and video." + Environment.NewLine;
				if (Convert.ToInt32(((InputBase)likesPerDayBox).get_Text()) > 500)
				{
					((InputBase)likesPerDayBox).set_Text("500");
				}
				if (Convert.ToInt32(((InputBase)followPerDayBox).get_Text()) > 50)
				{
					((InputBase)likesPerDayBox).set_Text("50");
				}
				if (Convert.ToInt32(((InputBase)unfollowPerDayBox).get_Text()) > 50)
				{
					((InputBase)likesPerDayBox).set_Text("50");
				}
				((InputBase)commentsPerDayBox).set_Text("0");
				DontUnfollowUserFollowYou.IsChecked = false;
				break;
			case 1:
				textBox = aboutBox;
				textBox.Text = textBox.Text + "Now you are using a \"GitHub\" version:" + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can like, follow, unfollow, comment without limits." + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can change most of settings." + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can't upload photo and video." + Environment.NewLine;
				DontUnfollowUserFollowYou.IsChecked = false;
				break;
			case 2:
				textBox = aboutBox;
				textBox.Text = textBox.Text + "Now you are using a \"Full\" version:" + Environment.NewLine;
				textBox = aboutBox;
				textBox.Text = textBox.Text + "You can do everything!" + Environment.NewLine;
				DontUnfollowUserFollowYou.IsEnabled = true;
				break;
			}
			aboutBox.Text += Environment.NewLine;
			textBox = aboutBox;
			textBox.Text = textBox.Text + "If you need to change version - please use the links below." + Environment.NewLine;
			enableAll();
			groupBoxLogin.IsEnabled = true;
		}

		private bool CheckTrial()
		{
			disableAll();
			if (trialBoxText.Text == "")
			{
				trialVText.Text = "Trial version";
				trialVText.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF9C3C3"));
				trialVText.Foreground = new SolidColorBrush(Colors.Red);
				TrialStatus = 0;
				WriteAboutText();
				return false;
			}
			string text = trialBoxText.Text;
			if (text == InstaBot.CalculateMD5Hash(InstaBot.CalculateMD5Hash("Super Git Hub Key 00001")))
			{
				trialVText.Text = "GitHub version";
				trialVText.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC3F9CD"));
				trialVText.Foreground = new SolidColorBrush(Colors.Green);
				TrialStatus = 1;
				WriteAboutText();
				return true;
			}
			if (text == InstaBot.CalculateMD5Hash(InstaBot.CalculateMD5Hash("Super One User Key For " + loginBox.Text)))
			{
				trialVText.Text = "Full version for " + loginBox.Text;
				trialVText.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC3E0F9"));
				trialVText.Foreground = new SolidColorBrush(Colors.Blue);
				TrialStatus = 2;
				WriteAboutText();
				return true;
			}
			if (text == InstaBot.CalculateMD5Hash(InstaBot.CalculateMD5Hash("Super Full Key 00001")))
			{
				trialVText.Text = "Full version";
				trialVText.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC3E0F9"));
				trialVText.Foreground = new SolidColorBrush(Colors.Blue);
				TrialStatus = 2;
				WriteAboutText();
				return true;
			}
			trialVText.Text = "Trial version";
			trialVText.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF9C3C3"));
			trialVText.Foreground = new SolidColorBrush(Colors.Red);
			TrialStatus = 0;
			WriteAboutText();
			return false;
		}

		private void trialBoxText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (!CheckTrial())
				{
					MessageBox.Show("Registration error! Now you using trial version.", "Registration check");
					return;
				}
				SaveSettings();
				MessageBox.Show("Thank you for your code, it is work!", "Registration check");
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			pbLoading.Visibility = Visibility.Hidden;
			LoadSettings();
			if (!CheckTrial())
			{
				MessageBox.Show("You using trial version. Read about tab to change it.", "Registration check");
			}
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			string text = Bot.GenerateComment();
			if (text == "")
			{
				MessageBox.Show("Look like you lost your comments.txt file...", "Comment example");
			}
			else
			{
				MessageBox.Show("Example: \"" + text + "\"", "Comment example");
			}
		}

		private void Int_Text_Input(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			try
			{
				textBox.Text = Convert.ToString(Convert.ToInt32(textBox.Text));
			}
			catch
			{
				textBox.Text = "0";
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/InstagramBot;component/mainwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Expected O, but got Unknown
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Expected O, but got Unknown
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Expected O, but got Unknown
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Expected O, but got Unknown
			switch (connectionId)
			{
			case 1:
				((MainWindow)target).Loaded += Window_Loaded;
				break;
			case 2:
				BotWindow = (Grid)target;
				break;
			case 3:
				tabControl = (TabControl)target;
				break;
			case 4:
				contentTab = (TabItem)target;
				break;
			case 5:
				groupBoxLogin = (GroupBox)target;
				break;
			case 6:
				loginBox = (TextBox)target;
				break;
			case 7:
				passwordBox = (PasswordBox)target;
				break;
			case 8:
				label1 = (Label)target;
				break;
			case 9:
				label2 = (Label)target;
				break;
			case 10:
				groupBoxAfterLogin = (GroupBox)target;
				break;
			case 11:
				photoImg_You = (Image)target;
				break;
			case 12:
				label3 = (Label)target;
				break;
			case 13:
				label3_Copy = (Label)target;
				break;
			case 14:
				label3_Copy1 = (Label)target;
				break;
			case 15:
				PFollowingC = (Label)target;
				break;
			case 16:
				PFollowerC = (Label)target;
				break;
			case 17:
				PMediaC = (Label)target;
				break;
			case 18:
				label = (Label)target;
				break;
			case 19:
				groupBox1 = (GroupBox)target;
				break;
			case 20:
				label4 = (Label)target;
				break;
			case 21:
				label6 = (Label)target;
				break;
			case 22:
				label7 = (Label)target;
				break;
			case 23:
				label8 = (Label)target;
				break;
			case 24:
				likesPerDayBox = (IntegerUpDown)target;
				((UpDownBase<int?>)(object)likesPerDayBox).add_ValueChanged((RoutedPropertyChangedEventHandler<object>)likesPerDayBox_ValueChanged);
				break;
			case 25:
				followPerDayBox = (IntegerUpDown)target;
				((UpDownBase<int?>)(object)followPerDayBox).add_ValueChanged((RoutedPropertyChangedEventHandler<object>)followPerDayBox_ValueChanged);
				break;
			case 26:
				unfollowPerDayBox = (IntegerUpDown)target;
				((UpDownBase<int?>)(object)unfollowPerDayBox).add_ValueChanged((RoutedPropertyChangedEventHandler<object>)unfollowPerDayBox_ValueChanged);
				break;
			case 27:
				commentsPerDayBox = (IntegerUpDown)target;
				((UpDownBase<int?>)(object)commentsPerDayBox).add_ValueChanged((RoutedPropertyChangedEventHandler<object>)commentsPerDayBox_ValueChanged);
				break;
			case 28:
				label4_Copy = (Label)target;
				break;
			case 29:
				label6_Copy = (Label)target;
				break;
			case 30:
				label7_Copy = (Label)target;
				break;
			case 31:
				label8_Copy = (Label)target;
				break;
			case 32:
				NextLikeTimeBox = (Label)target;
				break;
			case 33:
				NextFollowTimeBox = (Label)target;
				break;
			case 34:
				NextUnfollowTimeBox = (Label)target;
				break;
			case 35:
				NextCommentTimeBox = (Label)target;
				break;
			case 36:
				logBox = (TextBox)target;
				logBox.TextChanged += logBox_TextChanged;
				break;
			case 37:
				label5 = (Label)target;
				break;
			case 38:
				startButton = (Button)target;
				startButton.Click += startButton_click;
				break;
			case 39:
				stopButton = (Button)target;
				stopButton.Click += stopButton_click;
				break;
			case 40:
				workAnimation = (Rectangle)target;
				break;
			case 41:
				rotateThis = (RotateTransform)target;
				break;
			case 42:
				logoutButton = (Button)target;
				logoutButton.Click += logoutButton_Click;
				break;
			case 43:
				loginButton = (Button)target;
				loginButton.Click += loginButton_Click;
				break;
			case 44:
				pbLoading = (ProgressBar)target;
				break;
			case 45:
				mediaInfo = (GroupBox)target;
				break;
			case 46:
				label13 = (Label)target;
				break;
			case 47:
				label12 = (Label)target;
				break;
			case 48:
				labelInBotCommentsC = (Label)target;
				break;
			case 49:
				label10 = (Label)target;
				break;
			case 50:
				photoImg = (Image)target;
				break;
			case 51:
				labelInBotMediaId = (Label)target;
				break;
			case 52:
				label11 = (Label)target;
				break;
			case 53:
				labelInBotLikesC = (Label)target;
				break;
			case 54:
				label14 = (Label)target;
				break;
			case 55:
				((Hyperlink)target).Click += OnClick2;
				break;
			case 56:
				labelInBotUserT = (Run)target;
				break;
			case 57:
				((Hyperlink)target).Click += OnClick;
				break;
			case 58:
				labelInBotMediaUrlT = (Run)target;
				break;
			case 59:
				tabSettings = (TabItem)target;
				break;
			case 60:
				botAddSettings = (GroupBox)target;
				break;
			case 61:
				label9 = (Label)target;
				break;
			case 62:
				groupBox6 = (GroupBox)target;
				break;
			case 63:
				label21 = (Label)target;
				break;
			case 64:
				minLikesTB = (TextBox)target;
				minLikesTB.TextChanged += Int_Text_Input;
				break;
			case 65:
				label22 = (Label)target;
				break;
			case 66:
				maxLikesTB = (TextBox)target;
				maxLikesTB.TextChanged += Int_Text_Input;
				break;
			case 67:
				label23 = (Label)target;
				break;
			case 68:
				groupBox3 = (GroupBox)target;
				break;
			case 69:
				SaveFollowList = (CheckBox)target;
				break;
			case 70:
				groupBox2 = (GroupBox)target;
				break;
			case 71:
				label19 = (Label)target;
				break;
			case 72:
				FollowTime = (TextBox)target;
				FollowTime.TextChanged += Int_Text_Input;
				break;
			case 73:
				UnfollowOnlyCB = (CheckBox)target;
				break;
			case 74:
				DontUnfollowUserFollowYou = (CheckBox)target;
				break;
			case 75:
				groupBox4 = (GroupBox)target;
				break;
			case 76:
				CommentExample = (Button)target;
				CommentExample.Click += button_Click;
				break;
			case 77:
				groupBox5 = (GroupBox)target;
				break;
			case 78:
				label20 = (Label)target;
				break;
			case 79:
				maxLineLogBox = (TextBox)target;
				maxLineLogBox.TextChanged += Int_Text_Input;
				break;
			case 80:
				groupBox7 = (GroupBox)target;
				break;
			case 81:
				tagBox = (TextBox)target;
				tagBox.TextChanged += tagBox_TextChanged;
				break;
			case 82:
				groupBox7_Copy = (GroupBox)target;
				break;
			case 83:
				stopBox = (TextBox)target;
				stopBox.TextChanged += tagBox_TextChanged;
				break;
			case 84:
				apiSettings = (GroupBox)target;
				break;
			case 85:
				label16 = (Label)target;
				break;
			case 86:
				label15 = (Label)target;
				break;
			case 87:
				igSigKeyBox = (TextBox)target;
				break;
			case 88:
				igSigKeyVersion = (TextBox)target;
				break;
			case 89:
				label15_Copy = (Label)target;
				break;
			case 90:
				igSigUserAgent = (TextBox)target;
				break;
			case 91:
				tabAbout = (TabItem)target;
				break;
			case 92:
				aboutBox = (TextBox)target;
				break;
			case 93:
				label17 = (Label)target;
				break;
			case 94:
				trialBoxText = (TextBox)target;
				trialBoxText.KeyDown += trialBoxText_KeyDown;
				break;
			case 95:
				label18 = (Label)target;
				break;
			case 96:
				trialVText = (TextBlock)target;
				break;
			case 97:
				((Hyperlink)target).Click += OnClick3;
				break;
			case 98:
				onGitHub = (Run)target;
				break;
			case 99:
				((Hyperlink)target).Click += OnClick4;
				break;
			case 100:
				sendEmail = (Run)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}

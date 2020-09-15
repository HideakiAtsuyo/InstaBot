using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace InstagramBot
{
	internal class InstaBot
	{
		private string user_agent = "Instagram 9.4.0 Android (18/4.3; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)";

		private string api_url = "https://i.instagram.com/api/v1/";

		private string ig_sig_key = "fc4720e1bf9d79463f62608c86fbddd374cc71bbfb98216b52e3f75333bd130d";

		private string experiments = "ig_android_progressive_jpeg,ig_creation_growth_holdout,ig_android_report_and_hide,ig_android_new_browser,ig_android_enable_share_to_whatsapp,ig_android_direct_drawing_in_quick_cam_universe,ig_android_huawei_app_badging,ig_android_universe_video_production,ig_android_asus_app_badging,ig_android_direct_plus_button,ig_android_ads_heatmap_overlay_universe,ig_android_http_stack_experiment_2016,ig_android_infinite_scrolling,ig_fbns_blocked,ig_android_white_out_universe,ig_android_full_people_card_in_user_list,ig_android_post_auto_retry_v7_21,ig_fbns_push,ig_android_feed_pill,ig_android_profile_link_iab,ig_explore_v3_us_holdout,ig_android_histogram_reporter,ig_android_anrwatchdog,ig_android_search_client_matching,ig_android_high_res_upload_2,ig_android_new_browser_pre_kitkat,ig_android_2fac,ig_android_grid_video_icon,ig_android_white_camera_universe,ig_android_disable_chroma_subsampling,ig_android_share_spinner,ig_android_explore_people_feed_icon,ig_explore_v3_android_universe,ig_android_media_favorites,ig_android_nux_holdout,ig_android_search_null_state,ig_android_react_native_notification_setting,ig_android_ads_indicator_change_universe,ig_android_video_loading_behavior,ig_android_black_camera_tab,liger_instagram_android_univ,ig_explore_v3_internal,ig_android_direct_emoji_picker,ig_android_prefetch_explore_delay_time,ig_android_business_insights_qe,ig_android_direct_media_size,ig_android_enable_client_share,ig_android_promoted_posts,ig_android_app_badging_holdout,ig_android_ads_cta_universe,ig_android_mini_inbox_2,ig_android_feed_reshare_button_nux,ig_android_boomerang_feed_attribution,ig_android_fbinvite_qe,ig_fbns_shared,ig_android_direct_full_width_media,ig_android_hscroll_profile_chaining,ig_android_feed_unit_footer,ig_android_media_tighten_space,ig_android_private_follow_request,ig_android_inline_gallery_backoff_hours_universe,ig_android_direct_thread_ui_rewrite,ig_android_rendering_controls,ig_android_ads_full_width_cta_universe,ig_video_max_duration_qe_preuniverse,ig_android_prefetch_explore_expire_time,ig_timestamp_public_test,ig_android_profile,ig_android_dv2_consistent_http_realtime_response,ig_android_enable_share_to_messenger,ig_explore_v3,ig_ranking_following,ig_android_pending_request_search_bar,ig_android_feed_ufi_redesign,ig_android_video_pause_logging_fix,ig_android_default_folder_to_camera,ig_android_video_stitching_7_23,ig_android_profanity_filter,ig_android_business_profile_qe,ig_android_search,ig_android_boomerang_entry,ig_android_inline_gallery_universe,ig_android_ads_overlay_design_universe,ig_android_options_app_invite,ig_android_view_count_decouple_likes_universe,ig_android_periodic_analytics_upload_v2,ig_android_feed_unit_hscroll_auto_advance,ig_peek_profile_photo_universe,ig_android_ads_holdout_universe,ig_android_prefetch_explore,ig_android_direct_bubble_icon,ig_video_use_sve_universe,ig_android_inline_gallery_no_backoff_on_launch_universe,ig_android_image_cache_multi_queue,ig_android_camera_nux,ig_android_immersive_viewer,ig_android_dense_feed_unit_cards,ig_android_sqlite_dev,ig_android_exoplayer,ig_android_add_to_last_post,ig_android_direct_public_threads,ig_android_prefetch_venue_in_composer,ig_android_bigger_share_button,ig_android_dv2_realtime_private_share,ig_android_non_square_first,ig_android_video_interleaved_v2,ig_android_follow_search_bar,ig_android_last_edits,ig_android_video_download_logging,ig_android_ads_loop_count_universe,ig_android_swipeable_filters_blacklist,ig_android_boomerang_layout_white_out_universe,ig_android_ads_carousel_multi_row_universe,ig_android_mentions_invite_v2,ig_android_direct_mention_qe,ig_android_following_follower_social_context";

		private string sig_key_version = "4";

		private int error_400;

		private int error_400_to_ban = 3;

		private int ban_sleep_time = 7200;

		private List<int> bot_follow_list = new List<int>();

		private string log_file_path;

		private int log_file;

		private dynamic media_by_tag;

		private int media_by_tag_count;

		private bool login_status;

		private bool bot_working;

		private Dictionary<string, int> next_iteration = new Dictionary<string, int>
		{
			{
				"Like",
				0
			},
			{
				"Follow",
				0
			},
			{
				"Unfollow",
				0
			},
			{
				"Comment",
				0
			},
			{
				"LikeCounter",
				0
			},
			{
				"FollowCounter",
				0
			},
			{
				"UnfollowCounter",
				0
			},
			{
				"CommentCounter",
				0
			}
		};

		private Dictionary<string, int> follow_list = new Dictionary<string, int>();

		private bool save_follow_list;

		private bool dont_unfollow_if_user_follow_you;

		private string login;

		private string password;

		private string csrftoken;

		private string mid;

		private string sessionid;

		private string comment_list_from_file;

		private int like_per_day;

		private int media_max_like;

		private int media_min_like;

		private int follow_per_day;

		private int follow_time;

		private int unfollow_per_day;

		private int comments_per_day;

		private int max_like_for_one_tag;

		private int unfollow_break_min;

		private int unfollow_break_max;

		private int log_mod;

		private int like_delay;

		private int follow_delay;

		private int unfollow_delay;

		private int comments_delay;

		private string[] tag_list;

		private string[] stop_words;

		private string uuid;

		private string device_id;

		private string username_id;

		private string rank_token;

		private string last_response;

		private CookieContainer cookieC;

		private Random random = new Random();

		public void SetSettings(int like_per_day = 1000, int media_max_like = 10, int media_min_like = 0, int follow_per_day = 0, int follow_time = 3600, int unfollow_per_day = 0, int comments_per_day = 0, string[] tag_list = null, string[] stop_words = null, int max_like_for_one_tag = 5, int unfollow_break_min = 15, int unfollow_break_max = 30, int log_mod = 0, string ig_sig_key = "", string sig_key_version = "", string user_agent = "", bool save_follow_list = true, bool dont_unfollow_if_user_follow_you = false)
		{
			next_iteration["Like"] = 0;
			next_iteration["Follow"] = 0;
			next_iteration["Unfollow"] = 0;
			next_iteration["Comment"] = 0;
			this.ig_sig_key = ((ig_sig_key == "") ? this.ig_sig_key : ig_sig_key);
			this.sig_key_version = ((sig_key_version == "") ? this.sig_key_version : sig_key_version);
			this.user_agent = ((user_agent == "") ? this.user_agent : user_agent);
			if (tag_list == null || tag_list.Length == 0)
			{
				this.tag_list = new string[3]
				{
					"cat",
					"dog",
					"girl"
				};
			}
			else
			{
				this.tag_list = tag_list;
			}
			this.stop_words = stop_words;
			this.like_per_day = like_per_day;
			if (this.like_per_day > 0)
			{
				like_delay = 86400 / this.like_per_day;
			}
			this.media_max_like = media_max_like;
			this.media_min_like = media_min_like;
			this.follow_per_day = follow_per_day;
			if (this.follow_per_day > 0)
			{
				follow_delay = 86400 / this.follow_per_day;
			}
			this.follow_time = follow_time;
			this.unfollow_per_day = unfollow_per_day;
			if (this.unfollow_per_day > 0)
			{
				unfollow_delay = 86400 / this.unfollow_per_day;
			}
			this.comments_per_day = comments_per_day;
			if (this.comments_per_day > 0)
			{
				comments_delay = 86400 / this.comments_per_day;
			}
			this.max_like_for_one_tag = max_like_for_one_tag;
			this.unfollow_break_min = unfollow_break_min;
			this.unfollow_break_max = unfollow_break_max;
			this.log_mod = log_mod;
			this.save_follow_list = save_follow_list;
			this.dont_unfollow_if_user_follow_you = dont_unfollow_if_user_follow_you;
			LoadFollowList();
		}

		public async Task<bool> LoginFromFile(string login, string password)
		{
			this.login = login;
			this.password = password;
			string str = File.ReadAllText(Environment.CurrentDirectory + "\\data\\" + login + "-session.dat");
			try
			{
				string text = Decrypt(str, password);
				if (text != null)
				{
					dynamic val = JsonConvert.DeserializeObject(text);
					if (login == Convert.ToString(val.login) && password == Convert.ToString(val.password))
					{
						cookieC = new CookieContainer();
						cookieC.Add(new Cookie("csrftoken", Convert.ToString(val.csrftoken), "/", "i.instagram.com"));
						cookieC.Add(new Cookie("mid", Convert.ToString(val.mid), "/", "i.instagram.com"));
						cookieC.Add(new Cookie("sessionid", Convert.ToString(val.sessionid), "/", "i.instagram.com"));
						uuid = Convert.ToString(val.uuid);
						device_id = Convert.ToString(val.device_id);
						username_id = Convert.ToString(val.username_id);
						csrftoken = Convert.ToString(val.csrftoken);
						device_id = Convert.ToString(val.device_id);
						rank_token = Convert.ToString(val.rank_token);
						Console.WriteLine("Read from file ok!");
						if (await timelineFeed())
						{
							Console.WriteLine("timelineFeed ok!");
							if (await getv2Inbox())
							{
								Console.WriteLine("getv2Inbox ok!");
								if (await getRecentActivity())
								{
									Console.WriteLine("getRecentActivity ok!");
									if (await getUsernameInfoAsync(username_id))
									{
										dynamic val2 = JsonConvert.DeserializeObject(last_response);
										this.ShowBotMedia(val2, "profile");
									}
									return true;
								}
							}
						}
					}
				}
				else
				{
					WriteLog("Decrypt settings fail! Check your paasword!");
				}
			}
			catch (ArgumentNullException)
			{
				return false;
			}
			return false;
		}

		public async Task<bool> Login(string login, string password)
		{
			this.login = login;
			this.password = password;
			uuid = generateUUID(type: true);
			device_id = generateDeviceId();
			Console.WriteLine(uuid + " - " + device_id);
			cookieC = new CookieContainer();
			Console.WriteLine("try to login");
			if (await SendRequestAsync("si/fetch_headers/?challenge_type=signup&guid=" + generateUUID(type: false), null, login: true))
			{
				WriteLog("Handshake with Instagram success!");
				string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
				{
					{
						"phone_id",
						generateUUID(type: true)
					},
					{
						"_csrftoken",
						csrftoken
					},
					{
						"username",
						login
					},
					{
						"guid",
						uuid
					},
					{
						"device_id",
						device_id
					},
					{
						"password",
						password
					},
					{
						"login_attempt_count",
						"0"
					}
				}).ToString();
				if (await SendRequestAsync("accounts/login/", generateSignature(data), login: true))
				{
					login_status = true;
					rank_token = username_id + "_" + uuid;
					if (await syncFeatures())
					{
						Console.WriteLine("syncFeatures ok!");
						Console.WriteLine("getRecentActivity ok!");
						string value = JsonConvert.SerializeObject((object)new Dictionary<string, string>
						{
							{
								"login",
								login
							},
							{
								"password",
								password
							},
							{
								"username_id",
								username_id
							},
							{
								"csrftoken",
								csrftoken
							},
							{
								"uuid",
								uuid
							},
							{
								"device_id",
								device_id
							},
							{
								"rank_token",
								rank_token
							},
							{
								"mid",
								mid
							},
							{
								"sessionid",
								sessionid
							}
						}).ToString();
						new FileInfo(Environment.CurrentDirectory + "\\data\\").Directory.Create();
						File.WriteAllText(Environment.CurrentDirectory + "\\data\\" + login + "-session.dat", Encrypt(Convert.ToString(value), password));
						if (await getUsernameInfoAsync(username_id))
						{
							dynamic val = JsonConvert.DeserializeObject(last_response);
							this.ShowBotMedia(val, "profile");
						}
						return true;
					}
				}
			}
			return false;
		}

		private async Task<bool> likeAsync(string mediaId)
		{
			string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"_uuid",
					uuid
				},
				{
					"_uid",
					username_id
				},
				{
					"_csrftoken",
					csrftoken
				},
				{
					"media_id",
					mediaId
				}
			}).ToString();
			if (await SendRequestAsync("media/" + mediaId + "/like/", generateSignature(data)))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> followAsync(string userId)
		{
			string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"_uuid",
					uuid
				},
				{
					"_uid",
					username_id
				},
				{
					"user_id",
					userId
				},
				{
					"_csrftoken",
					csrftoken
				}
			}).ToString();
			if (await SendRequestAsync("friendships/create/" + userId + "/", generateSignature(data)))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> unfollowAsync(string userId)
		{
			string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"_uuid",
					uuid
				},
				{
					"_uid",
					username_id
				},
				{
					"user_id",
					userId
				},
				{
					"_csrftoken",
					csrftoken
				}
			}).ToString();
			if (await SendRequestAsync("friendships/destroy/" + userId + "/", generateSignature(data)))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> userFriendshipAsync(string userId)
		{
			if (await SendRequestAsync("friendships/show/" + userId + "/"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> checkForUserFollowYou(string userId)
		{
			if (await userFriendshipAsync(userId))
			{
				dynamic val = JsonConvert.DeserializeObject(last_response);
				if (val.followed_by == "True")
				{
					return true;
				}
			}
			return false;
		}

		public string DecodeFromUtf8(string utf8String)
		{
			byte[] array = new byte[utf8String.Length];
			for (int i = 0; i < utf8String.Length; i++)
			{
				array[i] = (byte)utf8String[i];
			}
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		private async Task<bool> commentAsync(string mediaId, string comment)
		{
			string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"_uuid",
					uuid
				},
				{
					"_uid",
					username_id
				},
				{
					"_csrftoken",
					csrftoken
				},
				{
					"comment_text",
					comment
				}
			}).ToString();
			if (await SendRequestAsync("media/" + mediaId + "/comment/", generateSignature(data)))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> getUsernameInfoAsync(string userId)
		{
			if (await SendRequestAsync("users/" + userId + "/info/"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> SendRequestAsync(string endpoint, string post = null, bool login = false)
		{
			Console.WriteLine("SendRequestAsync");
			try
			{
				Console.WriteLine("Try");
				HttpWebRequest request = WebRequest.CreateHttp(api_url + endpoint);
				request.CookieContainer = cookieC;
				request.KeepAlive = false;
				request.Accept = "*/*";
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
				request.Headers["Cookie2"] = "$Version=1";
				request.Headers["Accept-Language"] = "en-US";
				request.UserAgent = user_agent;
				if (post != null)
				{
					request.Method = "POST";
					byte[] bytes = Encoding.ASCII.GetBytes(post);
					request.ContentLength = bytes.Length;
					Stream obj = await request.GetRequestStreamAsync();
					obj.Write(bytes, 0, bytes.Length);
					obj.Close();
				}
				using HttpWebResponse httpWebResponse = (await request.GetResponseAsync()) as HttpWebResponse;
				if (login)
				{
					csrftoken = httpWebResponse.Cookies["csrftoken"].Value;
				}
				if (login && post != null)
				{
					username_id = httpWebResponse.Cookies["ds_user_id"].Value;
					sessionid = httpWebResponse.Cookies["sessionid"].Value;
				}
				if (login && post == null)
				{
					mid = httpWebResponse.Cookies["mid"].Value;
				}
				using Stream stream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(stream);
				string str = last_response = streamReader.ReadToEnd();
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					httpWebResponse.Close();
					streamReader.Close();
					return true;
				}
				WriteLog("Response: " + str);
				httpWebResponse.Close();
				streamReader.Close();
				return false;
			}
			catch (WebException ex)
			{
				WriteLog(ex.Message);
				return false;
			}
		}

		public async Task AutoMod()
		{
			bot_working = true;
			while (bot_working)
			{
				if (next_iteration["Like"] < DateNow() && like_per_day > 0)
				{
					next_iteration["Like"] = DateNow() * 2;
					if (await AutoModHelper("like"))
					{
						next_iteration["Like"] = DateNow() + Convert.ToInt32((double)like_delay * 0.8 + (double)like_delay * 0.2 * random.NextDouble());
					}
					else
					{
						next_iteration["Like"] = DateNow();
					}
				}
				if (next_iteration["Follow"] < DateNow() && follow_per_day > 0)
				{
					next_iteration["Follow"] = DateNow() * 2;
					if (await AutoModHelper("follow"))
					{
						next_iteration["Follow"] = DateNow() + Convert.ToInt32((double)follow_delay * 0.8 + (double)follow_delay * 0.2 * random.NextDouble());
					}
					else
					{
						next_iteration["Follow"] = DateNow();
					}
				}
				if (next_iteration["Unfollow"] < DateNow() && unfollow_per_day > 0)
				{
					next_iteration["Unfollow"] = DateNow() * 2;
					if (await AutoModHelper("unfollow"))
					{
						next_iteration["Unfollow"] = DateNow() + Convert.ToInt32((double)unfollow_delay * 0.8 + (double)unfollow_delay * 0.2 * random.NextDouble());
					}
					else
					{
						next_iteration["Unfollow"] = DateNow();
					}
				}
				if (next_iteration["Comment"] < DateNow() && comments_per_day > 0)
				{
					next_iteration["Comment"] = DateNow() * 2;
					if (await AutoModHelper("comment"))
					{
						next_iteration["Comment"] = DateNow() + Convert.ToInt32((double)comments_delay * 0.8 + (double)comments_delay * 0.2 * random.NextDouble());
					}
					else
					{
						next_iteration["Comment"] = DateNow();
					}
				}
				CallBackTime.callbackEventHandler(next_iteration);
				await Task.Delay(1000);
			}
		}

		public unsafe async Task<bool> AutoModHelper(string type)
		{
			if (media_by_tag_count > 0)
			{
				try
				{
					if (type != "unfollow")
					{
						if (!((media_by_tag[0].caption != null) ? true : false))
						{
							this.WriteLog("Media_id " + media_by_tag[0].pk + " have no text. Can't check for stop words...");
							this.ShowBotMedia(media_by_tag[0], type);
							media_by_tag.RemoveAt(0);
							media_by_tag_count--;
							return false;
						}
						if (this.CheckForStopWord(Convert.ToString(media_by_tag[0].caption.text)))
						{
							this.ShowBotMedia(media_by_tag[0], type);
							media_by_tag.RemoveAt(0);
							media_by_tag_count--;
							return false;
						}
					}
					AsyncTaskMethodBuilder<bool> asyncTaskMethodBuilder = default(AsyncTaskMethodBuilder<bool>);
					switch (type)
					{
					case "like":
					{
						Console.WriteLine("case Like " + media_by_tag[0].like_count);
						if (media_min_like > (int)media_by_tag[0].like_count || (int)media_by_tag[0].like_count > media_max_like)
						{
							this.WriteLog("This media like count [" + media_by_tag[0].like_count + "] not fit your settings!");
							this.ShowBotMedia(media_by_tag[0], type);
							media_by_tag.RemoveAt(0);
							media_by_tag_count--;
							return false;
						}
						this.WriteLog("Try to like media_id " + media_by_tag[0].pk + "...");
						if (_003C_003Eo__63._003C_003Ep__40 == null)
						{
							_003C_003Eo__63._003C_003Ep__40 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(InstaBot), new CSharpArgumentInfo[1]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = _003C_003Eo__63._003C_003Ep__40.Target;
						CallSite<Func<CallSite, object, bool>> _003C_003Ep__ = _003C_003Eo__63._003C_003Ep__40;
						dynamic val = this.likeAsync(Convert.ToString(media_by_tag[0].pk)).GetAwaiter();
						if (!(bool)val.IsCompleted)
						{
							ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
							if (awaiter == null)
							{
								INotifyCompletion awaiter2 = (INotifyCompletion)val;
								asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter2, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_0fe0: stateMachine*/);
							}
							else
							{
								asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_0ff3: stateMachine*/);
							}
							/*Error near IL_0ffc: leave MoveNext - await not detected correctly*/;
						}
						object arg = val.GetResult();
						if (target(_003C_003Ep__, arg))
						{
							next_iteration["LikeCounter"]++;
							this.WriteLog("Like #" + Convert.ToString(next_iteration["LikeCounter"]) + " on media_id " + media_by_tag[0].pk + " success!");
						}
						else
						{
							this.WriteLog("Like on media_id " + media_by_tag[0].pk + " fail!");
						}
						break;
					}
					case "follow":
					{
						this.WriteLog("Try to follow " + media_by_tag[0].user.username + "...");
						if (_003C_003Eo__63._003C_003Ep__70 == null)
						{
							_003C_003Eo__63._003C_003Ep__70 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(InstaBot), new CSharpArgumentInfo[1]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = _003C_003Eo__63._003C_003Ep__70.Target;
						CallSite<Func<CallSite, object, bool>> _003C_003Ep__ = _003C_003Eo__63._003C_003Ep__70;
						dynamic val = this.followAsync(Convert.ToString(media_by_tag[0].user.pk)).GetAwaiter();
						if (!(bool)val.IsCompleted)
						{
							ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
							if (awaiter == null)
							{
								INotifyCompletion awaiter2 = (INotifyCompletion)val;
								asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter2, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_1c35: stateMachine*/);
							}
							else
							{
								asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_1c48: stateMachine*/);
							}
							/*Error near IL_1c51: leave MoveNext - await not detected correctly*/;
						}
						object arg = val.GetResult();
						if (target(_003C_003Ep__, arg))
						{
							next_iteration["FollowCounter"]++;
							this.WriteLog("Follow #" + Convert.ToString(next_iteration["FollowCounter"]) + " " + media_by_tag[0].user.username + " success!");
							this.SaveFollowList(Convert.ToString(media_by_tag[0].user.pk), true);
						}
						else
						{
							this.WriteLog("Follow " + Convert.ToString(media_by_tag[0].user.username) + " fail!");
						}
						break;
					}
					case "unfollow":
					{
						if (follow_list == null)
						{
							break;
						}
						int num = 1999999999;
						string unfollow_user = "";
						foreach (KeyValuePair<string, int> item in follow_list)
						{
							if (item.Value + follow_time < DateNow() && item.Value < num)
							{
								num = item.Value;
								unfollow_user = item.Key;
							}
						}
						if (unfollow_user != "")
						{
							WriteLog("Try to unfollow user #" + unfollow_user + "...");
							if (dont_unfollow_if_user_follow_you && await checkForUserFollowYou(unfollow_user))
							{
								WriteLog("This user #" + unfollow_user + " - follow you, don't unfollow!");
								SaveFollowList(unfollow_user, add: false);
								return true;
							}
							if (await unfollowAsync(unfollow_user))
							{
								next_iteration["UnfollowCounter"]++;
								WriteLog("Unfollow #" + Convert.ToString(next_iteration["UnfollowCounter"]) + " success!");
								SaveFollowList(unfollow_user, add: false);
								return true;
							}
							WriteLog("Unfollow user #" + unfollow_user + " fail!");
							SaveFollowList(unfollow_user, add: false);
						}
						return false;
					}
					case "comment":
					{
						this.WriteLog("Try to comment media_id " + media_by_tag[0].pk + "...");
						string comment = GenerateComment();
						if (_003C_003Eo__63._003C_003Ep__98 == null)
						{
							_003C_003Eo__63._003C_003Ep__98 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(InstaBot), new CSharpArgumentInfo[1]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> target = _003C_003Eo__63._003C_003Ep__98.Target;
						CallSite<Func<CallSite, object, bool>> _003C_003Ep__ = _003C_003Eo__63._003C_003Ep__98;
						dynamic val = this.commentAsync(Convert.ToString(media_by_tag[0].pk), comment).GetAwaiter();
						if (!(bool)val.IsCompleted)
						{
							ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
							if (awaiter == null)
							{
								INotifyCompletion awaiter2 = (INotifyCompletion)val;
								asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter2, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_2a75: stateMachine*/);
							}
							else
							{
								asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref *(_003CAutoModHelper_003Ed__63*)/*Error near IL_2a88: stateMachine*/);
							}
							/*Error near IL_2a91: leave MoveNext - await not detected correctly*/;
						}
						object arg = val.GetResult();
						if (target(_003C_003Ep__, arg))
						{
							next_iteration["LikeCounter"]++;
							this.WriteLog("Comment #" + Convert.ToString(next_iteration["LikeCounter"]) + " on media_id " + media_by_tag[0].pk + " success!");
							WriteLog("You wrote: \"" + comment + "\"");
						}
						else
						{
							this.WriteLog("Comment on media_id " + media_by_tag[0].pk + " fail!");
						}
						break;
					}
					}
					this.ShowBotMedia(media_by_tag[0], type);
					media_by_tag.RemoveAt(0);
					media_by_tag_count--;
					return true;
				}
				catch (Exception ex)
				{
					WriteLog("Exception! " + ex.Message + ".");
					media_by_tag.RemoveAt(0);
					media_by_tag_count--;
				}
			}
			else
			{
				await getHashtagFeed(tag_list[random.Next(0, tag_list.Length)]);
				ResponseToIdArray(last_response);
			}
			return false;
		}

		public bool CheckForStopWord(string media_string)
		{
			media_string = media_string.ToLower();
			string[] array = stop_words;
			foreach (string text in array)
			{
				if (text != "" && media_string.Contains(text))
				{
					this.WriteLog("Find stop word \"" + text + "\" on media_id " + media_by_tag[0].pk + "!");
					return true;
				}
			}
			return false;
		}

		public void StopAutoMod()
		{
			bot_working = false;
		}

		public string GenerateComment()
		{
			string text = "";
			if (comment_list_from_file == null)
			{
				LoadCommentsList();
			}
			if (comment_list_from_file != null && comment_list_from_file != "")
			{
				string[] array = comment_list_from_file.Split(';');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(',');
					text += array2[random.Next(0, array2.Length)];
					if (i < array.Length - 2)
					{
						text += " ";
					}
				}
			}
			return text;
		}

		private void LoadCommentsList()
		{
			try
			{
				comment_list_from_file = File.ReadAllText(Environment.CurrentDirectory + "\\comments.dat");
			}
			catch
			{
				File.WriteAllText(Environment.CurrentDirectory + "\\comments.dat", "");
			}
		}

		private void LoadFollowList()
		{
			if (!save_follow_list)
			{
				return;
			}
			try
			{
				string text = File.ReadAllText(Environment.CurrentDirectory + "\\data\\" + login + "-bot-follow-list.dat");
				if (text != "")
				{
					follow_list = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);
				}
			}
			catch
			{
				string contents = "";
				new FileInfo(Environment.CurrentDirectory + "\\data\\").Directory.Create();
				File.WriteAllText(Environment.CurrentDirectory + "\\data\\" + login + "-bot-follow-list.dat", contents);
			}
		}

		private void SaveFollowList(string follow_user, bool add)
		{
			if (save_follow_list)
			{
				try
				{
					if (add)
					{
						follow_list.Add(follow_user, DateNow());
					}
					else
					{
						follow_list.Remove(follow_user);
					}
					string contents = JsonConvert.SerializeObject((object)follow_list).ToString();
					new FileInfo(Environment.CurrentDirectory + "\\data\\").Directory.Create();
					File.WriteAllText(Environment.CurrentDirectory + "\\data\\" + login + "-bot-follow-list.dat", contents);
				}
				catch
				{
				}
			}
			else
			{
				LoadFollowList();
			}
		}

		private async Task<bool> syncFeatures()
		{
			string data = JsonConvert.SerializeObject((object)new Dictionary<string, string>
			{
				{
					"_uuid",
					uuid
				},
				{
					"_uid",
					username_id
				},
				{
					"id",
					username_id
				},
				{
					"_csrftoken",
					csrftoken
				},
				{
					"experiments",
					experiments
				}
			}).ToString();
			if (await SendRequestAsync("qe/sync/", generateSignature(data)))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> autoCompleteUserList()
		{
			if (await SendRequestAsync("friendships/autocomplete_user_list/"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> timelineFeed()
		{
			if (await SendRequestAsync("feed/timeline/"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> getv2Inbox()
		{
			if (await SendRequestAsync("direct_v2/inbox/?"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> getRecentActivity()
		{
			if (await SendRequestAsync("news/inbox/?"))
			{
				return true;
			}
			return false;
		}

		private async Task<bool> getHashtagFeed(string hashtagString, string maxid = null)
		{
			WriteLog("Get media_id by tag: #" + hashtagString + ".");
			string endpoint = (maxid != null) ? ("feed/tag/" + hashtagString + "/?max_id=" + maxid + "&rank_token=" + rank_token + "&ranked_content=true&") : ("feed/tag/" + hashtagString + "/?rank_token=" + rank_token + "&ranked_content=true&");
			if (await SendRequestAsync(endpoint))
			{
				return true;
			}
			return false;
		}

		private void ResponseToIdArray(string response)
		{
			media_by_tag_count = 0;
			dynamic val = JsonConvert.DeserializeObject(response);
			media_by_tag = val.items;
			if (media_by_tag != null)
			{
				foreach (object item in media_by_tag)
				{
					_ = item;
					media_by_tag_count++;
				}
				WriteLog("Succes! Get " + media_by_tag_count + " media_id.");
				return;
			}
			media_by_tag = val.ranked_items;
			if (media_by_tag != null)
			{
				WriteLog("Bad #hashtag for boting, return just ranked media_id.");
				foreach (object item2 in media_by_tag)
				{
					_ = item2;
					media_by_tag_count++;
				}
				WriteLog("Succes! Get " + media_by_tag_count + " ranked media_id.");
			}
			else
			{
				WriteLog("Error! Can't get media_id...");
			}
		}

		public void WriteLog(string log)
		{
			string log2 = string.Concat(DateTime.Now, ". ", log);
			CallBackLog.callbackEventHandler(log2);
		}

		public void ShowBotMedia(dynamic media, string type)
		{
			CallBackMedia.callbackEventHandler(media, type);
		}

		public int DateNow()
		{
			return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		private string generateUUID(bool type)
		{
			string text = $"{random.Next(0, 65535):x4}{random.Next(0, 65535):x4}-{random.Next(0, 65535):x4}-{random.Next(0, 4095) | 0x4000:x4}-{random.Next(0, 16383) | 0x8000:x4}-{random.Next(0, 65535):x4}{random.Next(0, 65535):x4}{random.Next(0, 65535):x4}";
			if (!type)
			{
				return text.Replace("-", "");
			}
			return text;
		}

		private string generateDeviceId()
		{
			string arg = CalculateMD5Hash(login + password);
			DateTime lastWriteTime = File.GetLastWriteTime(Environment.CurrentDirectory);
			return "android-" + CalculateMD5Hash(arg + lastWriteTime).Substring(16);
		}

		public static string CalculateMD5Hash(string input)
		{
			MD5 mD = MD5.Create();
			byte[] bytes = Encoding.ASCII.GetBytes(input);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		private string generateSignature(string data)
		{
			string text = BitConverter.ToString(hmacSHA256(data, ig_sig_key)).Replace("-", "").ToLower();
			return "ig_sig_key_version=" + sig_key_version + "&signed_body=" + text + "." + WebUtility.UrlEncode(data);
		}

		private byte[] hmacSHA256(string data, string key)
		{
			using HMACSHA256 hMACSHA = new HMACSHA256(Encoding.UTF8.GetBytes(key));
			return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(data));
		}

		private string Encrypt(string str, string keyCrypt)
		{
			return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
		}

		private string Decrypt(string str, string keyCrypt)
		{
			try
			{
				CryptoStream cryptoStream = InternalDecrypt(Convert.FromBase64String(str), keyCrypt);
				StreamReader streamReader = new StreamReader(cryptoStream);
				string result = streamReader.ReadToEnd();
				cryptoStream.Close();
				cryptoStream.Dispose();
				streamReader.Close();
				streamReader.Dispose();
				return result;
			}
			catch (CryptographicException)
			{
				return null;
			}
		}

		private byte[] Encrypt(byte[] key, string value)
		{
			ICryptoTransform cryptoTransform = Rijndael.Create().CreateEncryptor(new PasswordDeriveBytes(value, null).GetBytes(16), new byte[16]);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
			cryptoStream.Write(key, 0, key.Length);
			cryptoStream.FlushFinalBlock();
			byte[] result = memoryStream.ToArray();
			memoryStream.Close();
			memoryStream.Dispose();
			cryptoStream.Close();
			cryptoStream.Dispose();
			cryptoTransform.Dispose();
			return result;
		}

		private CryptoStream InternalDecrypt(byte[] key, string value)
		{
			ICryptoTransform transform = Rijndael.Create().CreateDecryptor(new PasswordDeriveBytes(value, null).GetBytes(16), new byte[16]);
			return new CryptoStream(new MemoryStream(key), transform, CryptoStreamMode.Read);
		}
	}
}

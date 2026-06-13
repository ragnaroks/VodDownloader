using System;
using System.Text.Json.Serialization;

namespace VodDownloader.Entities;

public class VodDetailTemplate {
    [JsonPropertyName("vod_id")]
    public Int32 VodId { get; set; }

    [JsonPropertyName("type_id")]
    public Int32 TypeId { get; set; }

    [JsonPropertyName("type_id_1")]
    public Int32 TypeId1 { get; set; }

    [JsonPropertyName("group_id")]
    public Int32 GroupId { get; set; }

    [JsonPropertyName("vod_name")]
    public String VodName { get; set; } = String.Empty;

    [JsonPropertyName("vod_sub")]
    public String VodSub { get; set; } = String.Empty;

    [JsonPropertyName("vod_en")]
    public String VodEn { get; set; } = String.Empty;

    [JsonPropertyName("vod_status")]
    public Int32 VodStatus { get; set; }

    [JsonPropertyName("vod_letter")]
    public String VodLetter { get; set; } = String.Empty;

    [JsonPropertyName("vod_color")]
    public String VodColor { get; set; } = String.Empty;

    [JsonPropertyName("vod_tag")]
    public String VodTag { get; set; } = String.Empty;

    [JsonPropertyName("vod_class")]
    public String VodClass { get; set; } = String.Empty;

    [JsonPropertyName("vod_pic")]
    public String VodPic { get; set; } = String.Empty;

    [JsonPropertyName("vod_pic_thumb")]
    public String VodPicThumb { get; set; } = String.Empty;

    [JsonPropertyName("vod_pic_slide")]
    public String VodPicSlide { get; set; } = String.Empty;

    [JsonPropertyName("vod_pic_screenshot")]
    public String VodPicScreenshot { get; set; } = String.Empty;

    [JsonPropertyName("vod_actor")]
    public String VodActor { get; set; } = String.Empty;

    [JsonPropertyName("vod_director")]
    public String VodDirector { get; set; } = String.Empty;

    [JsonPropertyName("vod_writer")]
    public String VodWriter { get; set; } = String.Empty;

    [JsonPropertyName("vod_behind")]
    public String VodBehind { get; set; } = String.Empty;

    [JsonPropertyName("vod_blurb")]
    public String VodBlurb { get; set; } = String.Empty;

    [JsonPropertyName("vod_remarks")]
    public String VodRemarks { get; set; } = String.Empty;

    [JsonPropertyName("vod_pubdate")]
    public String VodPubdate { get; set; } = String.Empty;

    [JsonPropertyName("vod_total")]
    public Int32 VodTotal { get; set; }

    [JsonPropertyName("vod_serial")]
    public String VodSerial { get; set; } = String.Empty;

    [JsonPropertyName("vod_tv")]
    public String VodTv { get; set; } = String.Empty;

    [JsonPropertyName("vod_weekday")]
    public String VodWeekday { get; set; } = String.Empty;

    [JsonPropertyName("vod_area")]
    public String VodArea { get; set; } = String.Empty;

    [JsonPropertyName("vod_lang")]
    public String VodLang { get; set; } = String.Empty;

    [JsonPropertyName("vod_year")]
    public String VodYear { get; set; } = String.Empty;

    [JsonPropertyName("vod_version")]
    public String VodVersion { get; set; } = String.Empty;

    [JsonPropertyName("vod_state")]
    public String VodState { get; set; } = String.Empty;

    [JsonPropertyName("vod_author")]
    public String VodAuthor { get; set; } = String.Empty;

    [JsonPropertyName("vod_jumpurl")]
    public String VodJumpurl { get; set; } = String.Empty;

    [JsonPropertyName("vod_tpl")]
    public String VodTpl { get; set; } = String.Empty;

    [JsonPropertyName("vod_tpl_play")]
    public String VodTplPlay { get; set; } = String.Empty;

    [JsonPropertyName("vod_tpl_down")]
    public String VodTplDown { get; set; } = String.Empty;

    [JsonPropertyName("vod_isend")]
    public Int32 VodIsend { get; set; }

    [JsonPropertyName("vod_lock")]
    public Int32 VodLock { get; set; }

    [JsonPropertyName("vod_level")]
    public Int32 VodLevel { get; set; }

    [JsonPropertyName("vod_copyright")]
    public Int32 VodCopyright { get; set; }

    [JsonPropertyName("vod_points")]
    public Int32 VodPoints { get; set; }

    [JsonPropertyName("vod_points_play")]
    public Int32 VodPointsPlay { get; set; }

    [JsonPropertyName("vod_points_down")]
    public Int32 VodPointsDown { get; set; }

    [JsonPropertyName("vod_hits")]
    public Int32 VodHits { get; set; }

    [JsonPropertyName("vod_hits_day")]
    public Int32 VodHitsDay { get; set; }

    [JsonPropertyName("vod_hits_week")]
    public Int32 VodHitsWeek { get; set; }

    [JsonPropertyName("vod_hits_month")]
    public Int32 VodHitsMonth { get; set; }

    [JsonPropertyName("vod_duration")]
    public String VodMuration { get; set; } = String.Empty;

    [JsonPropertyName("vod_up")]
    public Int32 VodUp { get; set; }

    [JsonPropertyName("vod_down")]
    public Int32 VodDown { get; set; }

    [JsonPropertyName("vod_score")]
    public String VodScore { get; set; } = String.Empty;

    [JsonPropertyName("vod_score_all")]
    public Int32 VodScoreAll { get; set; }

    [JsonPropertyName("vod_score_num")]
    public Int32 VodScoreNum { get; set; }

    [JsonPropertyName("vod_time")]
    public String VodTime { get; set; } = String.Empty;

    [JsonPropertyName("vod_time_add")]
    public Int32 VodTimeAdd { get; set; }

    [JsonPropertyName("vod_time_hits")]
    public Int32 VodTimeHits { get; set; }

    [JsonPropertyName("vod_time_make")]
    public Int32 VodTimeMake { get; set; }

    [JsonPropertyName("vod_trysee")]
    public Int32 VodTrysee { get; set; }

    [JsonPropertyName("vod_douban_id")]
    public Int32 VodDoubanId { get; set; }

    [JsonPropertyName("vod_douban_score")]
    public String VodDoubanScore { get; set; } = String.Empty;

    [JsonPropertyName("vod_reurl")]
    public String VodReurl { get; set; } = String.Empty;

    [JsonPropertyName("vod_rel_vod")]
    public String VodRelVod { get; set; } = String.Empty;

    [JsonPropertyName("vod_rel_art")]
    public String VodRelArt { get; set; } = String.Empty;


    [JsonPropertyName("vod_pwd")]
    public String VodPwd { get; set; } = String.Empty;

    [JsonPropertyName("vod_pwd_url")]
    public String VodPwdUrl { get; set; } = String.Empty;

    [JsonPropertyName("vod_pwd_play")]
    public String VodPwdPlay { get; set; } = String.Empty;

    [JsonPropertyName("vod_pwd_play_url")]
    public String VodPwdPlayUrl { get; set; } = String.Empty;

    [JsonPropertyName("vod_pwd_down")]
    public String VodPwdDown { get; set; } = String.Empty;

    [JsonPropertyName("vod_pwd_down_url")]
    public String VodPwdDownUrl { get; set; } = String.Empty;

    [JsonPropertyName("vod_content")]
    public String VodContent { get; set; } = String.Empty;

    [JsonPropertyName("vod_play_from")]
    public String VodPlayFrom { get; set; } = String.Empty;

    [JsonPropertyName("vod_play_server")]
    public String VodPlayServer { get; set; } = String.Empty;

    [JsonPropertyName("vod_play_note")]
    public String VodPlayNote { get; set; } = String.Empty;

    [JsonPropertyName("vod_play_url")]
    public String VodPlayUrl { get; set; } = String.Empty;

    [JsonPropertyName("vod_down_from")]
    public String VodDownFrom { get; set; } = String.Empty;

    [JsonPropertyName("vod_down_server")]
    public String VodDownServer { get; set; } = String.Empty;

    [JsonPropertyName("vod_down_note")]
    public String VodDownNote { get; set; } = String.Empty;

    [JsonPropertyName("vod_down_url")]
    public String VodDownUrl { get; set; } = String.Empty;

    [JsonPropertyName("vod_plot")]
    public Int32 VodPlot { get; set; }

    [JsonPropertyName("vod_plot_name")]
    public String VodPlotName { get; set; } = String.Empty;

    [JsonPropertyName("vod_plot_detail")]
    public String VodPlotDetail { get; set; } = String.Empty;

    [JsonPropertyName("type_name")]
    public String TypeName { get; set; } = String.Empty;
}

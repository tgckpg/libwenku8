﻿namespace GR.Settings
{
	static class AppKeys
	{
		public const string
			// AvdDM
			DREG = FileLinks.ROOT_SETTING + FileLinks.ADM_DOWNLOAD_REG
			, DM_REQUESTID = "RequestID"
			, DM_PENDING = "Pending"
			, DM_ID = "ID"
			, DM_DESC = "Desc"
			, DM_REQUEST = "RequestUri"
			, DM_METHOD = "Method"
			, DM_FAILED_COUNT = "Failed"
			, DM_DEACTIVATED = "inactive"
			// Local Book Storage
			, LBS_ANCHOR = "anchor"
			, LBS_COLOR = "color"
			, LBS_DATE = "date"
			, LBS_DEL = "deleted"
			, LBS_CH = "chapter"
			, LBS_NEW = "new"
			, LBS_INDEX = "index"
			, LBS_AUM = "autoUpdate"
			, LBS_PINNED = "pinned"
			, LBS_WSYNC = "wSync"
			, LBS_TIME = "t"

			// XML Root Nodes
			, LBS_BXML = "<LocalBookStorage />"
			, LBS_AXML = "<Anchors />"
			, AdvDM_FXML = "<AdvDM />"
			, TS_CXML = "<Themes />"

			// Pin Manager
			, PM_POLICY = "policy"

			// Globals
			, GLOBAL_NAME = "name"
			, GLOBAL_ID = "id"
			// Chapter id
			, GLOBAL_CID = "cid"
			// Volume id
			, GLOBAL_VID = "vid"
			// Reply id
			, GLOBAL_RID = "rid"
			// User id
			, GLOBAL_UID = "uid"
			, GLOBAL_AID = "aid"

			, GLOBAL_SSID = "ssid"

			// Meta Tag
			, GLOBAL_META = "meta"

			// XML KEYS
			, XML_BINF_TITLE = "Title"
			, XML_BINF_AUTHOR = "Author"
			, XML_BINF_INTROPRV = "IntroPreview"
			, XML_BINF_BSTATUS = "BookStatus"
			, XML_BINF_LUPDATE = "LastUpdate"

			, XML_BMTA_PUSHCNT = "PushCount"
			, XML_BMTA_THITCNT = "TotalHitsCount"
			, XML_BMTA_DHITCNT = "DayHitsCount"
			, XML_BMTA_FAVCNT = "FavCount"
			, XML_BMTA_PRESSID = "PressId"
			, XML_BMTA_BLENGTH = "BookLength"
			, XML_BMTA_LSECTION = "LatestSection"

			, XML_UINFO_UNAME = "uname"
			, XML_UINFO_NNAWE = "nickname"
			, XML_UINFO_SCORE = "score"
			, XML_UINFO_EXP = "experience"
			, XML_UINFO_RANK = "rank"

			// Spider Props
			, BINF_INTRO = "Intro"
			, BINF_DATE = "Date"
			, BINF_OTHERS = "Others"
			, BINF_LENGTH = "Length"
			, BINF_STATUS = "Status"
			, BINF_LUPDATE = "LastUpdateDate"
			, BINF_PRESS = "Press"
			, BINF_ORGURL = "OriginalUrl"
			, BINF_COVER = "Cover"

			// AppGate Tags
			, APP_VERSION = "appver"
			, APP_REQUEST = "request"
			, APP_REQUEST_TOKEN = "timetoken"

			// Messaging
			, SH_SCRIPT_DATA = "SH_Script_Data"
			, SH_SCRIPT_REMOVE = "SH_Script_Remove"
			, SH_SHOW_GRANTS = "SH_Show_Grants"
			, HS_DECRYPT_FAIL = "HS_Decrypt_Fail"
			, HS_NO_VOLDATA = "HS_NoVolumeData"
			, HS_OPEN_COMMENT = "HS_Open_Comment"
			, HS_REPORT_SUCCESS = "HS_Report_Success"
			, HS_REPORT_FAILED = "HS_Report_Failed"
			, HSC_DECRYPT_FAIL = "HSC_Decrypt_Fail"
			, SP_PROCESS_COMP = "Spider_Process_Comp"
			, PM_CHECK_TILES = "PinManager_Check_Tiles"
			, PROMPT_LOGIN = "prompt-login"
			, ACQUIRE_SIG = "acquire-sig"

			, SEARCH_AUTHOR = "search-author"
			, OPEN_ZONE = "open-zone"
			, OPEN_VIEWSOURCE = "open-view-source"

			, EX_DEATHBLOW = "ExDeathblow"

			, ZLOCAL = "[Local]"

			// Background Task
			, BTASK_RETRY = "retry"
			, BTASK_SPIDER = "spider"

			, SYS_2ND_TILE_LAUNCH = "LaunchSecondaryTile"
			, SYS_FILE_LAUNCH = "LaunchFile"
			, SYS_EXCEPTION = "exception"
			, SYS_MESSAGE = "mesg"

			, UA = "Grimoire Reaper/{0} ( Windows; taotu engine; UAP ) wenku10 by Astropenguin"
			;

		public const char ANO_IMG = '\ufff9';
	}
}
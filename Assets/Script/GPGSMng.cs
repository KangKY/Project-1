using UnityEngine;
using GooglePlayGames;
using System;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    public static T GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    //Debug.Log("Nothing" + instance.ToString());
                    return null;
                }
            }
            return instance;
        }
    }
}

public class GPGSMng : Singleton<GPGSMng>
{
   


    /// <summary>
    /// 리더보드 아이디 값
    /// </summary>
    /// 

    public const String leader_board = "CgkIlNi06e8CEAIQCA";
    /// <summary>
    /// 현재 로그인 중인지 체크
    /// </summary>
    /// 
    public bool bLogin
    {
        get;
        set;
    }
    
        /// <summary>
        /// GPGS를 초기화 합니다.
        /// </summary>
        public void InitializeGPGS()
        {
            bLogin = false;

            PlayGamesPlatform.Activate();
        }

        /// <summary>
        /// GPGS를 로그인 합니다.
        /// </summary>
        public void LoginGPGS()
        {


            // 로그인이 안되어 있으면
            //if (!Social.localUser.authenticated)
                Social.localUser.Authenticate(LoginCallBackGPGS);
        }




        /// <summary>
        /// GPGS Login Callback
        /// </summary>
        /// <param name="result"> 결과 </param>
        public void LoginCallBackGPGS(bool result)
        {
            bLogin = result;
        }

        /// <summary>
        /// GPGS를 로그아웃 합니다.
        /// </summary>
        public void LogoutGPGS()
        {
            // 로그인이 되어 있으면
            if (Social.localUser.authenticated)
            {
                ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
                bLogin = false;
            }
        }

        /// <summary>
        /// GPGS에서 자신의 프로필 이미지를 가져옵니다.
        /// </summary>
        /// <returns> Texture 2D 이미지 </returns>
        public String GetIDGPGS()
        {
            if (Social.localUser.authenticated)
                return Social.localUser.id;
            else
                return null;
        }

        /// <summary>
        /// GPGS 에서 사용자 이름을 가져옵니다.
        /// </summary>
        /// <returns> 이름 </returns>
        public string GetNameGPGS()
        {
            if (Social.localUser.authenticated)
                return Social.localUser.userName;
            else
                return null;
        }

        /// <summary>
        /// 리더보드에 점수를 세팅
        /// </summary>
        /// <param name="score"></param>
        public void SetLeaderBoard(uint score)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, leader_board,
               (bool success) => {
                    //handle success or failure
                    if (success)
                   {

                   }
                   else
                   {

                   }
               });
            }


        }

        /// <summary>
        /// 해당하는 리더보드 UI를 보여줍니다.
        /// </summary>
        public void ShowLeaderBoard()
        {
            if (Social.localUser.authenticated)
            {
                ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leader_board);
            }
        }

        /// <summary>
        /// 모든 리더보드를 보여줍니다.
        /// </summary>
        public void ShowLeaderBoardAll()
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
        }

        /// <summary>
        /// 업적 시스템 UI를 보여줍니다.
        /// </summary>
        public void ShowAchievementUI()
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
            }
        }

        /// <summary>
        /// 해당하는 업적을 언락시켜줍니다.
        /// </summary>
        /// <param name="nKey"></param>
        public void UnLockAchievement(int nKey)
        {
            if (Social.localUser.authenticated)
            {
                switch (nKey)
                {
                    case 0:
                        // 달릴시간이다! 게임 첫 시작!
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_its_time_to_run_yaaaap, 100.0f,
                           (bool success) => {
                               if (success) { }
                               else { }
                           });
                        break;
                    case 1:
                        // 1 스테이지에서 바로 죽을 때..
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_come_out_person_who_said_easy, 100.0f,
                          (bool success) => {
                              if (success) { }
                              else { }
                          });
                        break;
                    case 2:
                        // 10회 게임
                        PlayGamesPlatform.Instance.IncrementAchievement(
                            Yaaaap.GooglePlay.achievement_im_yaaaap_beginner, 1, (bool success) => {
                                if (success) { }
                                else { }
                            });

                        break;
                    case 3:
                        // 30회 게임
                        PlayGamesPlatform.Instance.IncrementAchievement(
                            Yaaaap.GooglePlay.achievement_im_running_man, 1, (bool success) => {
                                if (success) { }
                                else { }
                            });
                        break;
                    case 4:
                        // 100회 게임
                        PlayGamesPlatform.Instance.IncrementAchievement(
                            Yaaaap.GooglePlay.achievement_im_marathoner, 1, (bool success) => {
                                if (success) { }
                                else { }
                            });
                        break;
                    case 5:
                        // 동시 점프 한번도 없이 2번째 스테이지 도달
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_staggered_you_and_me, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 6:
                        // 동시 점프 한번도 없이 4번째 스테이지 도달
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_we_should_break_up, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 7:
                        // 동시 점프 10회 성공
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_this_is_teamwork, 100.0f,
                         (bool success) => {
                             if (success) { }
                             else { }
                         });
                        break;
                    case 8:
                        // 동시 점프 50회 성공
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_im_you_you_are_me, 100.0f,
                       (bool success) =>{
                        if (success){}
                           else{}});
                        break;
                    case 9:
                        // 동시 점프 100회 성공
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_fusion, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });

                        break;
                    case 10:
                        // 점수 100점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_its_now_being_adapted, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 11:
                        // 점수 300점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_adaptation_is_over, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 12:
                        // 점수 500점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_run_is_not_over, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 13:
                        // 점수 800점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_you_are_invincible_man, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 14:
                        // 점수 900점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_you_are_legend, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 15:
                        // 점수 1000점 달성
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_you_are_yaaaap_conqueror, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 16:
                        // 성 스테이지 입장
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_welcome_castle, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;

                    case 17:

                        // 하늘 스테이지 입장
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_welcome_sky, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 18:
                        // 바다 스테이지 입장
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_welcome_sea, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 19:
                        // 불지옥 스테이지 입장
                        Social.ReportProgress(Yaaaap.GooglePlay.achievement_welcome_hell, 100.0f,
                       (bool success) => {
                           if (success) { }
                           else { }
                       });
                        break;
                    case 20:
                        break;

                }
            }

        }
}
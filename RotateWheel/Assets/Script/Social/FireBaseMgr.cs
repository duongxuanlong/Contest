// using System.Collections;
// using Firebase.Unity;
using UnityEngine;
using Firebase.Unity.Editor;
using Firebase.Auth;
using Firebase.Database;

public class FireBaseMgr 
{
    #region fields
    Firebase.FirebaseApp m_App;
    FirebaseUser m_User;

    static FireBaseMgr m_Instance;
    const string APP_URL = "https://magicsign-20e21.firebaseio.com/";
    const string LEADER_BOARD = "leaderboard";
    #endregion

    #region private methods
    FireBaseMgr()
    {
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(APP_URL);
        m_User = null;
    }
    #endregion

    #region public methods
    public static FireBaseMgr Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FireBaseMgr();
            return m_Instance;
        }
    }
    public void CheckLatestVersion ()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var status = task.Result;
            if (status == Firebase.DependencyStatus.Available)
            {
                this.m_App = Firebase.FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.Log("Can't use FireBase in here");
            }
        });
    }

    public void SignInFireBase (string accessToken)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Credential credential = FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithCredentialAsync was cancelled");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.Log("SignInWithCredentialAsync encountered an error: " + task);
                return;
            }

            m_User = task.Result;
            Debug.LogFormat("User signed in successfully with: {0} and {1}", m_User.DisplayName, m_User.UserId);
        });
    }

    public void SignOutFireBase ()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    public void PostMe ()
    {
        LeaderBoardEntry entry = new LeaderBoardEntry();
        entry.username = m_User.DisplayName;
        entry.email = m_User.Email;
        entry.bestscore = GameController.m_Instance.GetBestScore();

        string str = JsonUtility.ToJson(entry);
        Debug.LogFormat ("str: {0}", str);

        FirebaseDatabase.DefaultInstance.GetReference(LEADER_BOARD).SetRawJsonValueAsync(str);
    }

    // public LeaderBoardEntry GetMe ()
    // {

    // }
    #endregion
}
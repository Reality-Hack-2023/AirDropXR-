using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class PhoneAppManager : MonoBehaviour
{
   
    public Button Return; 
    public GameObject FlickObject; 
    //public GameObject ObjectsSelectorObject; 
    public GameObject EditorObject; 
    public GameObject ObjectSelector;
    public GameObject FileTypeSelector;
    public GameObject DrawObject;
    public GameObject DrawButtonHolder;
    public FlickInteraction FlickInterator;
    public Image flickimage;
   public NavigationBar NavModeChanger;

   //drawing references
    public postRequestMain NetworkSender;
   public SpriteRenderer DrawImageSpriteRnd;

    private ContentDataSync _contentDataSync;
    
    public Touch touch;

    public ContentScript[] ContentButtons = new ContentScript[0];
    public FileTypeScript[] FileTypeButtons = new FileTypeScript[0];
    public ReturnScript[] ReturnButtons = new ReturnScript[0];
    public CompleteScript[] CompleteButtons = new CompleteScript[0];

   bool _hasDrawImageSprite = false;

    private float Swipevalue;
    private Vector2 point;
    private ContentScript _selectedContent = null;
    private FileTypeScript _selectedFileType = null;
    private ReturnScript _selectedReturnType = null;

    public static PhoneAppManager I { get; private set; }
    void Awake()
    {
        I = this;

        foreach(var cb in ContentButtons)
        {
            Debug.Log("adding cb");
            cb.OnButtonPressed.AddListener(_OnContentButtonPressed);
        }
        foreach(var ftb in FileTypeButtons)
        {
            Debug.Log("adding ftb");
            ftb.OnButtonPressed.AddListener(_OnFileTypeButtonPressed);
        }
        foreach(var rb in ReturnButtons)
        {
            Debug.Log("adding rb");
            rb.OnButtonPressed.AddListener(_OnReturnButtonPressed);
        }
        foreach(var cpb in CompleteButtons)
        {
            Debug.Log("adding cpb");
            cpb.OnButtonPressed.AddListener(_OnCompleteButtonPressed);
        }    

        if(FlickInterator)
        FlickInterator.OnFlicked.AddListener(OnFlick);

      if (NavModeChanger)
         NavModeChanger.OnNavModeChange.AddListener(_OnNavModeChanged);
    }

    public enum state
    {
        None, ObjectSelecetor, Flick, Editor, FileTypeSelecetor, Draw, 
    }
    private state _currentState = state.None;
    // Start is called before the first frame update
    void Start()
    {
        SetState(state.FileTypeSelecetor);
       
    }
    void SetState(state newstate)
    {
        if(_currentState == newstate) return;

        _currentState = newstate;

        Debug.Log("state " + _currentState);

        FlickObject.SetActive(newstate == state.Flick);
        EditorObject.SetActive(newstate == state.Editor);
        ObjectSelector.SetActive(newstate == state.ObjectSelecetor);
        FileTypeSelector.SetActive(newstate == state.FileTypeSelecetor);
        DrawObject.SetActive(newstate == state.Draw);
        DrawButtonHolder.SetActive(newstate == state.Draw);

      //reset draw image flag so we dont get stuck flicking the old draw image
      if (newstate == state.ObjectSelecetor)
         _hasDrawImageSprite = false;
    }

   void _OnNavModeChanged(int newNavMode)
   {
      Debug.Log("Nav mode changed to: " + newNavMode);
      if (_contentDataSync)
         _contentDataSync.ManipulationMode = newNavMode;
   }

    void _OnContentButtonPressed(ContentScript  b)
    {
            //int contentId = b.ContentID;
            //Button button = b.GetButton();

            _selectedContent = b;
            SetState(state.Flick);
            if(flickimage)
               flickimage.sprite = _selectedContent.contentImage.sprite;
    }
    void _OnFileTypeButtonPressed(FileTypeScript  b)
    {
            //int contentId = b.ContentID;
            //Button button = b.GetButton();
            Debug.Log("filetypebuttonpressed");
            _selectedFileType = b;
            foreach(var cb in ContentButtons)
            {
                cb.RefreshImage(_selectedFileType.FileTypeID);
                cb.gameObject.SetActive(cb.contentImage.sprite != null);
            }
            int fileTypeId = b.FileTypeID;
            if(fileTypeId == 0) {
                SetState(state.ObjectSelecetor);
            }
            if(fileTypeId == 1) {
                SetState(state.ObjectSelecetor);
            }
            if(fileTypeId == 2) {
                SetState(state.ObjectSelecetor);
            }
            if(fileTypeId == 3) {
                SetState(state.Draw);
            }            
    }
    void _OnReturnButtonPressed(ReturnScript  b)
    {
            //int contentId = b.ContentID;
            //Button button = b.GetButton();
            if(_contentDataSync)
            {
            _contentDataSync.IsManipulating = false;
            }

            _selectedReturnType = b;
            SetState(state.FileTypeSelecetor);
    }
    void _OnCompleteButtonPressed(CompleteScript b) {
        Debug.Log("Drawing Complete");

      _hasDrawImageSprite = true;
      if (flickimage)
         flickimage.sprite = DrawImageSpriteRnd.sprite;

      SetState(state.Flick);
    }
    
    void OnFlick()
    {   
        Debug.Log("OnFlick" + _currentState);

        if(_currentState != state.Flick ) return;

         if(NetworkMgr.I.GetIsMultiplayerSession() ) 
        {
            
             
            GameObject spawnedContentObject = Realtime.Instantiate("ContentNetSyncer", ownedByClient: true);
            _contentDataSync = spawnedContentObject.GetComponent<ContentDataSync>();
            spawnedContentObject.GetComponent<RealtimeTransform>().RequestOwnership();

            if(_selectedContent)
            {
               _contentDataSync.ContentId = _selectedContent.ContentID;
               _contentDataSync.ContentType = _selectedFileType.FileTypeID;
               Debug.Log(_contentDataSync.RotationOffset);
               Debug.Log(_contentDataSync.TranslationOffset);
               Debug.Log(_contentDataSync.SizeOffset);

               Sprite flickSprite = (_hasDrawImageSprite && DrawImageSpriteRnd) ? DrawImageSpriteRnd.sprite : _selectedContent.contentImage.sprite;

               if (flickimage)
                  flickimage.sprite = flickSprite;
            }
            // SetPositionValue(0.0f);
            // SetRotationValue(0.0f);
            // SetSizeValue(0.0f);

            //if we have a drawing, communicate the drawing number to vr app
         if(_hasDrawImageSprite && NetworkSender && _contentDataSync)
         {
            int curImageNum = NetworkSender.inc;
            _contentDataSync.RemoteImageNum = curImageNum;
            _contentDataSync.ContentType = 0;
            _contentDataSync.ContentId = 0;
         }

            RealtimeView myObjectView = spawnedContentObject.GetComponent<RealtimeView>();
            myObjectView.RequestOwnership();
            
            
            SetState(state.Editor);

        }
       
    
    }
    public void SetRotationValue(float newvalue)
    {
        

        if(_contentDataSync)
        {
            
            _contentDataSync.RotationOffset = newvalue;
            Vector3 curPos = _contentDataSync.transform.localPosition;
            curPos.x = newvalue;
            _contentDataSync.transform.localPosition = curPos;
        }
    }
    public void SetPositionValue(float newvalue)
    {
        if(_contentDataSync)
        {
            _contentDataSync.TranslationOffset = newvalue;
            Vector3 curPos = _contentDataSync.transform.localPosition;
            curPos.y = newvalue;
            _contentDataSync.transform.localPosition = curPos;
        }
    }
    public void SetSizeValue(float newvalue)
    {
        if(_contentDataSync)
        {
            _contentDataSync.SizeOffset = newvalue;
            Vector3 curPos = _contentDataSync.transform.localPosition;
            curPos.z = newvalue;
            _contentDataSync.transform.localPosition = curPos;
        }
    }
    void Update()
    {
        
    }
}

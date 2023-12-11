using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    [DefaultExecutionOrder(-1)]
    public class SceneUI : SceneService
    {
        [Header("Starter Page")]
        [SerializeField]
        private EnumId starterPage;
        [SerializeField]
        private List<UIPage> _stackedPages;

        private Dictionary<EnumId, UIPage> _cachedPages;

        private void Awake()
        {
            _cachedPages = new Dictionary<EnumId, UIPage>();
            _stackedPages = new List<UIPage>();

            PreparePages();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.2f);
            
            if(starterPage != null)
                PushPage(starterPage);
        }

        public void PushPage(EnumId pageId)
        {
            PushPage(pageId, null);
        }

        public void PushPage(EnumId pageId, PageData data)
        {
            StartCoroutine(PushPageCoroutine(pageId, data));
        }
        
        public void PopToFirstPage()
        {
            if(starterPage != null)
                PopToPage(_stackedPages[_stackedPages.Count - 1].PageID);
        }

        public void PopToPage(EnumId pageId)
        {
            if (pageId == null)
            {
                Debug.Log($"Page ID is Null");
                return;
            }

            UIPage targetPage = null;
            List<UIPage> toBePoppedPage = new List<UIPage>();

            for (int i = 0; i < _stackedPages.Count; i++)
            {
                UIPage page = _stackedPages[i];

                if (page.PageID.IsEqual(pageId) == false)
                {
                    toBePoppedPage.Add(page);
                }
                else
                {
                    targetPage = page;
                    break;
                }
            }

            for (int i = 0; i < toBePoppedPage.Count; i++)
            {
                UIPage poppedPage = toBePoppedPage[i];
                poppedPage.Close();
                _stackedPages.Remove(poppedPage);
            }

            if (targetPage == null)
                Debug.LogWarning($"Can't found Page {pageId.name}. Stack Empty");
            else
                targetPage.Open();
        }

        public void PopPage()
        {
            StartCoroutine(PopPageCoroutine());
        }

        public UIPage GetPage(EnumId pageId)
        {
            if (_cachedPages.ContainsKey(pageId))
                return _cachedPages[pageId];

            return null;
        }

        private UIPage GetTopPage()
        {
            if (_stackedPages.Count > 0)
                return _stackedPages[0];

            return null;
        }

        private void PreparePages()
        {
            var pages = GetComponentsInChildren<UIPage>(true);

            foreach (var page in pages)
            {
                page.gameObject.SetActive(true);
                if (page.PageID == null)
                {
                    Debug.LogError($"Page id of Page {page.name} is Null. Can't add to dictionary.");
                    continue;
                }

                page.SetupPage(this);
                page.InstantClose();

                _cachedPages.TryAdd(page.PageID, page);
            }
        }
    
        private IEnumerator PushPageCoroutine(EnumId pageId, PageData data)
        {
            if (GetPageFromStack(pageId) != null)
            {
                Debug.LogWarning(string.Format("Screen {0} already exists in the stack. Ignoring push request.", pageId.name));
                yield break;
            }

            if (_cachedPages.TryGetValue(pageId, out UIPage pushedPage) == false)
            {
                // Or maybe spawn the page?
                Debug.Log($"Page {pageId} doesn't exist");
                yield break;
            }

            // Set current top page condition
            UIPage topPage = GetTopPage();
            if (topPage != null && pushedPage.DisablePreviousPage)
            {
                topPage.Close();
                yield return new WaitUntil(() => { return topPage.IsCloseAnimationIsPlaying == false; });
            }            

            // Change top page to new page
            _stackedPages.Insert(0, pushedPage);
            pushedPage.OnPush(data);
            pushedPage.Open();
        }

        private IEnumerator PopPageCoroutine()
        {
            UIPage pageToPop = GetTopPage();
            if (pageToPop != null)
            {
                _stackedPages.RemoveAt(0);
                pageToPop.Close();
                yield return new WaitUntil(() => { return pageToPop.IsCloseAnimationIsPlaying == false; });
            }

            UIPage newTopPage = GetTopPage();
            if (newTopPage != null)
            {
                if (pageToPop.DisablePreviousPage)
                    newTopPage.Open();
                else
                    newTopPage.Refresh();
            }
        }

        private UIPage GetPageFromStack(EnumId pageId)
        {
            UIPage result = null;
            foreach (var page in _stackedPages)
            {
                if (page.PageID.IsEqual(pageId))
                {
                    result = page;
                    break;
                }
            }

            return result;
        }
    }    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System;

public class Window_Graph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<int> cantElements;

    private void Awake(){
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        cantElements = new List<int>() {150000, 300000, 500000, 700000, 1000000, 1200000, 1500000, 1700000, 2000000};

        //CreateCircle(new Vector2(200, 200));
        //List<int> valueList = new List<int>() {100, 1000, 5000, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000};
        ShowGraph(cantElements, (int _i) => ""+(_i), (float _f) => Mathf.RoundToInt(_f) + "s");

        int n;
        /*foreach (int cantidad in cantElements){
            valueList.Clear();
            for(int i = 0; i<cantidad;i++){
                valueList.Add(UnityEngine.Random.Range(0,500));
            }
            ShowGraph(valueList, (int _i) => (_i), (float _f) => Mathf.RoundToInt(_f) + ¨ secs¨);
        }*/

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject CreateCircle(Vector2 anchoredPosition){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null){
        if(getAxisLabelX == null){
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if(getAxisLabelY == null){
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 100f;
        float yMaximun = 20f;

        GameObject lastCircleGameObject = null;
        for(int i = 0; i < valueList.Count; i++){
            float xPosition = xSize + i * xSize;
            float yPosition = (calcularY(valueList[i])/yMaximun) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if(lastCircleGameObject != null){
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -3f);
            labelX.GetComponent<Text>().text = getAxisLabelX(valueList[i]);

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f/separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximun);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
    }
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB){
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    private float calcularY(int pElementos){
        List<int> valueList = new List<int>();
        for(int i = 0; i<pElementos;i++){
            valueList.Add(UnityEngine.Random.Range(0,500));
        }
        return quickSort(valueList);
    }

    private float selectionSort(List<int> pListaElementos){
        DateTime inicio = DateTime.Now;
        int indiceMenor;
        int n = pListaElementos.Count;
        for (int i = 0; i < n-1; i++){
            indiceMenor = i;
            for (int j = i+1; j < n; j++){
                if(pListaElementos[j] < pListaElementos[indiceMenor]){
                    indiceMenor = j;
                }
                if(i != indiceMenor){
                    intercambiar(pListaElementos, i, indiceMenor);
                }
            }
        }
        DateTime final = DateTime.Now;
        TimeSpan duracion = final - inicio;
        return (float)duracion.Seconds;;
    }

    private void intercambiar(List<int> pLista, int posA, int posB){
        int tempNum = pLista[posA];
        pLista[posA] = pLista[posB];
        pLista[posB] = tempNum;
    }

    public float quickSort(List<int> pLista){
        return quickSort(pLista, 0, (pLista.Count)-1);
    }

    private float quickSort(List<int> pListaElementos, int primero, int ultimo){
        DateTime inicio = DateTime.Now;
        int central = (primero + ultimo)/2;
        int pivote = pListaElementos[central];
        int i = primero;
        int j = ultimo;
        do{
            while (pListaElementos[i] < pivote) i++;
            while (pListaElementos[j] > pivote) j--;
            if(i <= j){
                intercambiar(pListaElementos, i, j);
                i++;
                j--;
            }
        }while(i <= j);
        if(primero < j){
            quickSort(pListaElementos, primero, j);
        }if(i < ultimo){
            quickSort(pListaElementos, i, ultimo);
        }
        DateTime final = DateTime.Now;
        TimeSpan duracion = final - inicio;
        return (float)duracion.Seconds;;
    }

}

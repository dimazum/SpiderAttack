//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;


////public enum CharState
////{
////    Idle,
////    Run,
////    Rubilovo,
////    IdleBlink,
////    IdleKirche,
////    IdleKircheBlink,
////    WalkKirche

////}


//public class moveManager : MonoBehaviour
//{

//    private Rigidbody2D rb;
//    private float move;
//    private float moveV;//перемещение из инпута
//    public bool richtDirect;         //направление
//    public float speed = 2f;                  //скорость



//    public gameController gc;
//    public CaveEntrance cE;
//    public Trebuchet Tt;

//    Vector3 CnPosition;
//    public Game current;

//   // public Text alertText;
//    GameObject blockLR;
//    GameObject blockUp;
//    GameObject blockDown;
//    public GameObject currentBlock;
//    public GameObject currentBlockDead;
//    public GameObject currentBlock2;
//    public GameObject target;

//    public Camera CameraMain;
//    public GameObject Trees;
//    public GameObject Forest;
//    public GameObject Field1;
//    public GameObject Field2;

//    public float speedField1 = 1;
//    public float speedField1Vertical = 1;
//    public float xPosField1;
//    public float yPosField1;
//    public float zPosField1;

//    public float speedField2;
//    public float speedField2Vertical = 1;
//    public float xPosField2;
//    public float yPosField2;
//    public float zPosField2;


//    public float speedForest = 1;
//    public float speedForestVertical = 1;
//    public float xPosForest;
//    public float yPosForest;
//    public float zPosForest;

//    public float speedTrees = 1;
//    public float speedTreesVertical = 1;
//    public float xPosLes;
//    public float yPosLes;
//    public float zPosLes;

//    public float xPos;
//    public float yPos;
//    public float zPos;

//    public Transform Portal; //точка телепорта на базу
//    public GameObject explosion; //анмация хватания объекта

//    public GameObject stairs;  //ссылка на лестницу
//    public GameObject support; //ссылка на опору
//    public GameObject bomb;//ссылка на бомбу

//    public Inventary Inventary;
//    public LayerMask tumanMask;

//    public Sprite[] destroySp;

//    public Animator animator;
//    private bool isGrounded; //нахождение на земле
//    private bool canUP;      //возможность ползти по лестнице
//    private bool canMove;
//    private bool canMoveUp; //есть ли сверху препятствие
//    //private bool onStairs;
//    //private bool lastStairs;
//    private bool underMeStairs;
//    public bool Test = false;

//    public CharState state
//    {
//        get { return (CharState)animator.GetInteger("State"); }
//        set { animator.SetInteger("State", (int)value); }
//    }



//    void Awake()
//    {
//        Application.targetFrameRate = -1;
//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//        richtDirect = false;

//        isGrounded = false;
//        canUP = false;
//        canMove = false;
//        current = new Game();
//    }

//    void Start()
//    {
//    }

//    private void FixedUpdate()
//    {


//    }
//    private void CheckGround()
//    {
//        Collider2D[] coll = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.0f), 0.05f, 1 << 9);

//        Collider2D checkStairs = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.1f), 1 << 9);
//        underMeStairs = false;
//        if (checkStairs != null)
//            if (checkStairs.tag == "stairs")
//                underMeStairs = true;


//        //OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.3f), 0.05f, 1 << 9);

//        isGrounded = coll.Length > 0; //я на земле

//        //если один из коллайдеров лестница, то могу лезть
//        canUP = false;
//        foreach (Collider2D colliders in coll)
//            if (colliders.tag == "stairs")
//            {
//                canUP = true;
//                //Debug.Log("Могу вверх!");
//            }
//        //Debug.Log("Кличество коллайдеров: " + coll.Length + ", на земле: " + isGrounded + ", подомной лестница: " + underMeStairs);
//    }




//    public void LoadScene()
//    {
//        if (File.Exists("savedGames.gd"))
//        {
//            BinaryFormatter bfLoad = new BinaryFormatter();
//            FileStream fileLoad = File.Open("savedGames.gd", FileMode.Open);
//            current = (Game)bfLoad.Deserialize(fileLoad);
//            fileLoad.Close();

//            int startX_CSV = -50;
//            int startY_CSV = -1;
//            int num = 0;
//            for (int j = startY_CSV; j > startY_CSV - 100; j--)
//            {
//                for (int i = startX_CSV; i < startX_CSV + 100; i++)
//                {
//                    Vector2 newPos = new Vector2(current.x[num], current.y[num]);
//                    Instantiate(gc.element[0], newPos, Quaternion.identity);
//                    //Debug.Log(current.n[num]);
//                    if (current.n[num] > 0)
//                    {
//                        GameObject gm = gc.element[current.n[num]];
//                        Instantiate(gm, newPos, Quaternion.identity);
//                    }
//                    num++;
//                    //Debug.Log("Ставлю!");
//                }
//            }
//            //Debug.Log("Я загрузился!");
//        }
//        else
//        {
//            Debug.Log("Не могу найти файл сохранения!");
//        }
//    }



//    IEnumerator BombDestroy(Collider2D[] colladers, Vector2 mousePos)
//    {
//        //делаем задержку для анимации бомбы
//        yield return new WaitForSeconds(1.2f);
//        GameObject currentGO;

//        //подгружаем спрайты дестроя
//        for (int i = 0; i < destroySp.Length; i++)
//        {
//            foreach (Collider2D item in colladers)
//            {
//                currentGO = item.gameObject;
//                currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = destroySp[i];//спрайт для дестроя
//            }
//            //yield return new WaitForFixedUpdate();
//            yield return new WaitForSeconds(0.09f);
//        }

//        //"уничтожаем" блоки
//        foreach (Collider2D item in colladers)
//        {
//            currentGO = item.gameObject;
//            currentGO.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;//спрайт фон
//            currentGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;//спрайт для дестроя
//            currentGO.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;// спрайт ресурсов
//            currentGO.SetActive(false);
//            //currentGO.GetComponent<BoxCollider2D>().isTrigger = true;
//            //currentGO.layer = 10; //перемещаем блок на мертвый слой, с которым у игрока нет физической коллизии;
//            //Destroy(item);
//        }

//        //движение того, что сверху после взрыва
//        //указатель в верхний левый угол блока из девяти элментов
//        mousePos.x = mousePos.x - 1;
//        mousePos.y = mousePos.y + 1;
//        for (int i = 0; i < 3; i++)
//        {
//            Vector2 pos = mousePos;
//            Vector2 direction = Vector2.up;
//            float distance = 0.6f;
//            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 9); //стреляем вверх
//            if (hit.collider != null)
//            {

//                if (hit.collider.name.Contains("Ground")) //если попали не в землю, то двигаем то что свреху
//                //    if (hit.collider.tag != "blockGround")
//                {
//                    IMoveDown gm = hit.collider.GetComponent<IMoveDown>();
//                    //if (hit.collider.tag == "stoneDynamic") //если попали в камень, то вызваем дрожание камеры и движение блок с задержкой
//                    if (hit.collider.name.Contains("stone"))
//                    {
//                        Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать блок!");
//                        gm.MoveObjectDown(2f);
//                    }
//                    else
//                    {
//                        //Debug.Log("Я взорванный " + i + "-й, пытаюсь двигать, то что сверху!");
//                        gm.MoveObjectDown(0);
//                    }
//                }
//            }
//            mousePos.x++;
//        }
//    }

//    void Update()
//    {
//        CheckGround(); //чекаем землю
//        if (!isGrounded && !underMeStairs) //если на земле, включаем гравитацию, иначе выключаем
//            rb.gravityScale = 4f;
//        else
//        {
//            rb.gravityScale = 0;
//            rb.bodyType = RigidbodyType2D.Static;
//            rb.bodyType = RigidbodyType2D.Dynamic;
//        }

//        //бомба
//        //if (Input.GetMouseButtonDown(1))
//        //{
//        //    Dinamit();
//        //}


//        //сохраняю текущее состояние карты
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            int startX_CSV = -50;
//            int startY_CSV = -1;
//            int num = 0;
//            for (int j = startY_CSV; j > startY_CSV - 100; j--)

//            {
//                for (int i = startX_CSV; i < startX_CSV + 100; i++)
//                {
//                    current.x[num] = i;
//                    current.y[num] = j;
//                    Collider2D check = Physics2D.OverlapPoint(new Vector2(i, j), 1 << 9);
//                    if (check != null)
//                    {
//                        switch (check.tag)
//                        {
//                            case "blockGround":
//                                current.n[num] = 1;
//                                //Debug.Log("Сохраняю землю!");
//                                break;
//                            case "stoneDynamic":
//                                current.n[num] = 2;
//                                break;
//                            case "stairs":
//                                current.n[num] = 3;
//                                break;
//                            case "support":
//                                current.n[num] = 4;
//                                break;
//                            default:
//                                current.n[num] = 0;
//                                //Debug.Log("Сохраняю блок!");
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        current.n[num] = 0;
//                        //Debug.Log("Пусто! "+ Game.current.x[num]+" "+ Game.current.y[num]+" "+ Game.current.n[num]);
//                    }
//                    num++;
//                }
//            }

//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Create("savedGames.gd");
//            bf.Serialize(file, current);
//            file.Close();
//            Debug.Log("Я сохранился!");

//        }

//        //восстановление из файла
//        //if (Input.GetKeyDown(KeyCode.Z))
//        //{
//        //    LoadScene();
//        //    //SceneManager.LoadScene(0);
//        //}

//        ////restart
//        //if (Input.GetKeyDown(KeyCode.N))
//        //{
//        //    SceneManager.LoadScene(0);
//        //}


//        //удаление элемента
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            float posX = Mathf.Round(transform.position.x);
//            float posY = Mathf.Round(transform.position.y) + 1;
//            Vector2 pos = new Vector2(posX, posY);
//            RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9);//пускаем луч вниз с единицу выше по слою блоков
//            if (InstHit.collider != null)  //если лестница или опора, то уничтожаем объект и двигаем все сверху
//                if (InstHit.collider.tag == "stairs" || InstHit.collider.tag == "support")
//                {
//                    Destroy(InstHit.collider.gameObject);
//                    pos = new Vector2(pos.x, pos.y - 1);
//                    //стреляем вверх и двигаем, то что сверху если не земля
//                    InstHit = Physics2D.Raycast(pos, Vector2.up, 1f, 1 << 9);
//                    if (InstHit.collider != null)
//                        //if (InstHit.collider.name.Contains("Ground"))
//                        //if (InstHit.collider.tag != "blockGround")
//                        if (InstHit.collider.GetComponent<blockGround>())
//                        {
//                            IMoveDown gm = InstHit.collider.GetComponent<IMoveDown>();
//                            gm.MoveObjectDown(0);
//                        }
//                }
//        }

//        //установка лестницы
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            Stair();
//        }

//        //установка опоры
//        if (Input.GetKeyDown(KeyCode.O))
//        {
//            Opora();
//        }


//        //движение по горизонтали
//        //if (Input.GetButton("Horizontal"))
//        //{
//        //move = Input.GetAxis("Horizontal");
//        //Debug.Log(move);
//        if (Mathf.Abs(move) > 0.6f)
//        {
//            // Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x-Mathf.Sign(transform.localScale.x)*0.25f, transform.position.y+0.2f), 1 << 9);
//            Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x - Mathf.Sign(transform.localScale.x) * 0.4f, transform.position.y + 0.2f), 1 << 9);
//            canMove = true;
//            if (check != null)
//                if (check.tag != "stairs" && check.tag != "support")
//                    canMove = false;

//            if (canMove)
//            {
//                Vector2 dirLR = transform.right * move;
//                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dirLR, speed * Time.deltaTime);
//            }

//            if (cE.inCave == false)
//                state = CharState.Run;
//            else if (cE.inCave == true)
//                state = CharState.WalkKirche;

//            Vector2 playerPosition = transform.position;
//            Vector2 direction = -Vector2.right * transform.localScale.x;
//            float distance = 0.4f;

//            RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, distance, 1 << 9);

//            if (hit.collider != null)
//            {
//                //if (hit.collider.name.Contains("Ground"))
//                //if (hit.collider.tag == "blockGround")
//                if (hit.collider.GetComponent<blockGround>())
//                {
//                    currentBlock = hit.collider.gameObject;
//                    state = CharState.Rubilovo;
//                }
//            }
//            else currentBlock = null;

//            if (move > 0 && !richtDirect) Flip();
//            if (move < 0 && richtDirect) Flip();
//        }

//        //else if ((Random.Range(0, 100) < 100) && cE.inCave == false)
//        //{
//        //    state = CharState.Idle;
//        //}
//        ////else if ((Random.Range(0, 100) < 100) && cE.inCave == false)
//        ////{
//        ////    state = CharState.IdleBlink;
//        //}
//        else if (cE.inCave == true)
//            state = CharState.IdleKirche;

//        // Движение по вертикали
//        //Debug.Log(moveV);
//        if (Mathf.Abs(moveV) > 0.6f)
//        {
//            //move = Input.GetAxis("Vertical");

//            //если могу лезть, то лезу

//            Vector2 playerPosition = transform.position;
//            float distance = 1f;


//            if (moveV > 0) //вверх
//            {

//                Collider2D check = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.5f), 1 << 9);
//                canMoveUp = true;
//                if (check != null)
//                {


//                    if (check.tag != "stairs")
//                    {
//                        //Debug.Log("Не могу лезть вверх!");
//                        canMoveUp = false;//если чтото есть свеху, то не могу головой биться
//                    }
//                    else
//                    {
//                        //Debug.Log("Могу лезть!");
//                    }
//                }
//                else
//                {
//                   // Debug.Log("Пусто!");
//                }




//                if (canUP && canMoveUp)
//                {
//                    //Debug.Log("Лезу вверх!");
//                    if (cE.inCave == false)
//                        state = CharState.Run;
//                    else state = CharState.WalkKirche;

//                    Vector2 dirUP = transform.up * moveV;
//                    transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dirUP, 0.5f * speed * Time.deltaTime);
//                }


//                Vector2 direction = Vector2.up;
//                RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, distance, 1 << 9);

//                if (hit.collider != null)
//                {
//                    //if (hit.collider.name.Contains("Ground"))
//                    //if (hit.collider.tag == "blockGround")
//                    if (hit.collider.GetComponent<blockGround>())
//                    {
//                        currentBlock = hit.collider.gameObject;
//                        state = CharState.Rubilovo;
//                    }
//                }
//            }
//            else
//            if (moveV < 0)
//            {

//                if (underMeStairs)
//                {
//                    if (cE.inCave == false) state = CharState.Run;
//                    else state = CharState.WalkKirche;
//                    Vector2 dirDown = transform.up * moveV;
//                    transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dirDown, 0.5f * speed * Time.deltaTime);
//                }

//                Vector2 direction = Vector2.down;
//                distance = 0.6f;
//                RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, distance, 1 << 9);

//                if (hit.collider != null)
//                {
//                    //if (hit.collider.name.Contains("Ground"))
//                    //if (hit.collider.tag == "blockGround")
//                    if (hit.collider.GetComponent<blockGround>())
//                    {
//                        currentBlock = hit.collider.gameObject;
//                        state = CharState.Rubilovo;
//                    }
//                }
//            }
//        }

//    }
//    //void LateUpdate()
//    //{
//    //    CameraMain.transform.position = new Vector3(target.transform.position.x + xPos, target.transform.position.y + yPos, target.transform.position.z + zPos);
//    //    Trees.transform.position = new Vector3(target.transform.position.x * speedTrees + xPosLes, target.transform.position.y * speedTreesVertical + yPosLes, Trees.transform.position.z);
//    //    Forest.transform.position = new Vector3(target.transform.position.x * speedForest + xPosForest, target.transform.position.y * speedForestVertical + yPosForest, Forest.transform.position.z);
//    //    Field1.transform.position = new Vector3(target.transform.position.x * speedField1 + xPosField1, target.transform.position.y * speedField1Vertical + yPosField1, Field1.transform.position.z);
//    //    Field2.transform.position = new Vector3(target.transform.position.x * speedForest + xPosField2, target.transform.position.y * speedField2Vertical + yPosField2, Field2.transform.position.z);
//    //}

//    public void TrebucheteZaryad()
//    {
//        if (Tt.GetComponent<Trebuchet>().inTrebucheteTrigger)
//            animator.Play("TrebucheteZaryad");
//    }
//    public void TrebucheteZaryadOff()
//    {
//        animator.Play("idle");
//    }
//    public void TrebuchetePovorotRight()
//    {
//        if (Tt.GetComponent<Trebuchet>().inTrebucheteTrigger)
//            animator.Play("TrebuchetePovorotRight");
//    }
//    public void TrebuchetePovorotLeft()
//    {
//        if (Tt.GetComponent<Trebuchet>().inTrebucheteTrigger)
//            animator.Play("TrebuchetePovorotLeft");
//    }
//    public void Dinamit()
//    {
//        float posX = Mathf.Round(transform.position.x);
//        float posY = Mathf.Round(transform.position.y);
//        Vector2 pos = new Vector2(posX, posY);

//        bomb.transform.position = pos;//перемистили бомбу в точку;
//        bomb.GetComponent<Animator>().SetTrigger("runBomb");

//        Collider2D[]
//        colladers = Physics2D.OverlapCircleAll(pos, 1.1f, 1 << 9);
//        StartCoroutine(BombDestroy(colladers, pos));
//    }
//    public void Opora()
//    {
//        float posX = Mathf.Round(transform.position.x);
//        float posY = Mathf.Round(transform.position.y) + 1;
//        Vector2 pos = new Vector2(posX, posY);

//        RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9); //пускаем луч по слою блоков
//        if (InstHit.collider == null)
//        {
//            pos.y = pos.y - 1;
//            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider == null)
//            {
//                Debug.Log("Не могу поставить опору в воздухе!");
//            }
//            else
//            {
//                if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider.tag == "stairs")
//                {
//                    Debug.Log("Не могу поставить опору на лестнице!");
//                }
//                else
//                {
//                    Debug.Log("Место свободно, могу поставить опору!");
//                    Instantiate(support, pos, Quaternion.identity); //если ничего не нашли, то ставим опору
//                }
//            }
//        }
//        else Debug.Log("Место занято, не могу поставить опору!");
//    }
//    public void Teleport()
//    {
//        state = CharState.Idle;
//        transform.position = Portal.position;

//    }
//    public void Stair()
//    {
//        float posX = Mathf.Round(transform.position.x);
//        float posY = Mathf.Round(transform.position.y) + 1;
//        Vector2 pos = new Vector2(posX, posY);


//        RaycastHit2D InstHit = Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9);//пускаем луч вниз с единицу выше по слою блоков

//        if (InstHit.collider == null) //если место свободно, проверяем не на воздухе ли ставим лестницу
//        {
//            pos.y = posY - 1; //возвращаемся в центр установки
//            if (Physics2D.Raycast(pos, Vector2.down, 1f, 1 << 9).collider == null)//стреляем вниз
//            {
//                Debug.Log("Не могу поставить лестницу в воздухе!");
//            }
//            else
//            {
//                //Debug.Log("Место свободно, могу поставить лестницу!");
//                Instantiate(stairs, pos, Quaternion.identity); //если ничего не нашли внизу не пусто, то ставим лестницу
//            }
//        }
//        else
//        {
//            Debug.Log("Место занято, не могу поставить лестницу! Стоит: " + InstHit.collider.tag);
//        }
//    }

//    void Flip()
//    {
//        richtDirect = !richtDirect;
//        Vector3 scale = transform.localScale;
//        scale.x *= -1;
//        transform.localScale = scale;
//    }


//    void SpriteNull()
//    {
//        explosion.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
//    }


//    public void counter()
//    {
//        if (currentBlock != null)
//        {

//            blockGround bg = currentBlock.GetComponent<blockGround>();
//            bg.count++;
//            if (bg.count == 4)
//            {

//                RaycastHit2D hitUp = Physics2D.Raycast(currentBlock.transform.position, Vector2.up, 1f, 1 << 9);
//                RaycastHit2D hitDown = Physics2D.Raycast(currentBlock.transform.position, Vector2.down, 1f, 1 << 9);
               

//                currentBlock.gameObject.layer = 10; //перемещаем блок на мертвый слой, с которым у игрока нет физической коллизии;
//                bg.destroySpriteR.sprite = null;
//                bg.fon.sprite = null;
                

//                //анимация исчезновения ресурса, если уничтожили не землю
//                if (currentBlock.GetComponent<Item>())
//                {


//                    explosion.transform.position = currentBlock.transform.position;
//                    explosion.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bg.resource.sprite;
//                    explosion.GetComponent<Animator>().SetTrigger("trigger");


//                }
//                //if (currentBlock.GetComponent<Item>())
//                //{
//                //    Test = true;
//                //    currentBlock2 = currentBlock;
//                //    //Debug.Log(currentBlock2.gameObject);
//                //}
               
//                //прячем спрайт анимации исчезновения, после того как отыграет анимация
//                Invoke("SpriteNull", 0.36f);

//                bg.resource.sprite = null;
//                if (hitUp.collider != null && hitDown.collider != null)
//                {
//                    bg.sopli.sprite = destroySp[3];

//                }
//                else if (hitUp.collider != null && hitDown.collider == null)
//                {
//                    bg.sopli.sprite = destroySp[4];
//                    RaycastHit2D hitDownDead = Physics2D.Raycast(currentBlock.transform.position, Vector2.down, 1f, 1 << 10);
//                    currentBlockDead = hitDownDead.collider.gameObject;
//                    blockGround bgDead = currentBlockDead.GetComponent<blockGround>();
//                    if (bgDead.sopli.sprite == destroySp[3])
//                    {
//                        bgDead.sopli.sprite = destroySp[5];
//                    }
//                    else
//                    {
//                        bgDead.sopli.sprite = null;
//                    }


//                }
//                else if (hitUp.collider == null && hitDown.collider != null)
//                {
//                    bg.sopli.sprite = destroySp[5];
//                    RaycastHit2D hitUpDead = Physics2D.Raycast(currentBlock.transform.position, Vector2.up, 1f, 1 << 10);

//                    if (hitUpDead)
//                    {
//                        currentBlockDead = hitUpDead.collider.gameObject;
//                        blockGround bgDead = currentBlockDead.GetComponent<blockGround>();

//                        if (bgDead.sopli.sprite == destroySp[3])
//                        {
//                            bgDead.sopli.sprite = destroySp[4];
//                        }
//                        else
//                        {
//                            bgDead.sopli.sprite = null;
//                        }

//                    }
//                }
//                else if (hitUp.collider == null && hitDown.collider == null)
//                {
//                    bg.sopli.sprite = null;
//                    RaycastHit2D hitUpDead = Physics2D.Raycast(currentBlock.transform.position, Vector2.up, 1f, 1 << 10);
//                    RaycastHit2D hitDownDead = Physics2D.Raycast(currentBlock.transform.position, Vector2.down, 1f, 1 << 10);
//                    if (hitUpDead)
//                    {
//                        GameObject currentBlockDeadUp = hitUpDead.collider.gameObject;
//                        blockGround bgDeadUp = currentBlockDeadUp.GetComponent<blockGround>();
//                        if (bgDeadUp.sopli.sprite == destroySp[3])
//                        {
//                            bgDeadUp.sopli.sprite = destroySp[4];
//                        }
//                        else
//                        {
//                            bgDeadUp.sopli.sprite = null;
//                        }
//                    }
//                    if (hitDownDead)
//                    {
//                        GameObject currentBlockDeadDown = hitDownDead.collider.gameObject;
//                        blockGround bgDeadDown = currentBlockDeadDown.GetComponent<blockGround>();
//                        if (bgDeadDown.sopli.sprite == destroySp[3])
//                        {
//                            bgDeadDown.sopli.sprite = destroySp[5];
//                        }
//                        else
//                        {
//                            bgDeadDown.sopli.sprite = null;
//                        }
//                    }


//                }
//                //Debug.Log("Interfase!!!");
//                IMoveDown gm = currentBlock.GetComponent<IMoveDown>();
//                gm.MoveObjectDown(0);

//                //подготавливаем строку для поиска в словаре, т.к. объекты имеют (Clone) в названии
//                string str = currentBlock.name;
//                int num = str.IndexOf("(Clone)");
//                if (num >= 0)
//                    str = currentBlock.name.Remove(num, "(Clone)".Length);

//                //если в словаре ресурсов есть такой ресур, который мы разрушили,
//                if (gameState.resursDictionary.ContainsKey(str))
//                {
//                    //если цена этого ресура не равна нулю
//                    if (gameState.resursDictionary[str] != 0)
//                    {
//                        if (gameState.currentPlayer.bagDictionary.ContainsKey(str))
//                        {
//                            gameState.currentPlayer.bagDictionary[str]++;
//                        }
//                        else
//                        {
//                            gameState.currentPlayer.bagDictionary.Add(str, 1);
//                        }
//                    }
//                }
//                if (currentBlock.GetComponent<Item>())
//                {
//                    Inventary.SendMessage("IdentificationItem", currentBlock);


//                }
//            }

//            //currentBlock = null;
//            else
//            {
//                bg.destroySpriteR.sprite = destroySp[bg.count - 1];
//            }
            

//        }



//        //отнимаем жизни на 1
//        //gameState.currentPlayer.Health--;




//    }
//}





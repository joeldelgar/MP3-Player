using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace projecte20h
{
    class Program
    {
        const int MAX = 100;
        public class Cancion
        {
            public string nombre;
            public int numerocancion;
            public string autor;
        }
        // Lista de Canciones 
        public class ListaCanciones
        {
            public Cancion[] canciones = new Cancion[MAX];
            public int num;
        }

        //------------------------------FUNCIÓN PARA MOSTRAR LA LISTA DE CANCIONES--------------------------------------------------------------------------

        static void escribirCanciones(ListaCanciones lista)
        // Escribe en pantalla el contenido de la lista de canciones que recibe como parámetro 
        {
            Console.WriteLine("Estas son las canciones que hay en la lista: ");
            int i = 0;
            while (i < lista.num)//Des de la primera hasta la ultima cancion de la lista, escribimos por pantalla el nombre y el autor de cada canción
            {
                Console.WriteLine("{0}.{1}-{2}", i + 1, lista.canciones[i].nombre, lista.canciones[i].autor);
                i++;
            }
            Console.WriteLine();
        }

        //--------------------------FUNCIÓN PARA INTRODUCIR CANCIÓN-----------------------------------------------------------------------------------------

        static int ponCancion(ListaCanciones lista, Cancion c)
        //Añade una cancion a la lista 
        {
            if (lista.num < MAX)//Miramos si la lista está llena, si no lo está, añadimos la canción en la ultima posicion de la lista.
            {
                lista.canciones[lista.num] = c;
                lista.num++;//El numero de canciones de la lista incrementa en 1
                return 0;//Si la canción ha sido añadida correctamente, la función devuelve un 0
            }
            else//Si la lista si que está llena, la función devuelve un -1
            {
                return -1;
            }
        }

        //-------------------------FUNCIÓN PARA ELIMINAR CANCIÓN--------------------------------------------------------------------------------------------

        static void quitaCancion(ListaCanciones lista, int numero)
        {
            int i = numero - 1;//El numero de cancion es la posicion + 1, debido a que el numero de cancion empieza por 1, y las posiciones por 0
            while (i < lista.num)//Recorremos el vector des de la posicion donde se encuentra la cancion que eliminamos, hasta la ultima, poniendo en cada posicion el contenido de la posicion siguiente
            {
                lista.canciones[i] = lista.canciones[i + 1];
                i++;
            }
            lista.num--;//Al haber eliminado una cancion de la lista, el numero de canciones es uno menos
        }

        //--------------------------FUNCIÓN PARA DESCARGAR DATOS EN EL FICHERO----------------------------------------------------------------------------------

        static void guardarDatos(ListaCanciones lista)
        {
            StreamWriter F = new StreamWriter("D:\\canciones.txt");
            int i = 0;

            while (i < lista.num)
            {
                F.WriteLine(i + 1 + "." + lista.canciones[i].nombre + "-" + lista.canciones[i].autor);
                i++;
            }
            F.Close();

        }

        //-----------------------FUNCIÓN PARA CARGAR DATOS------------------------------------------------------------------------------------------------------

        static int cargarDatos(ListaCanciones lista)
        //leemos los datos que hay en canciones.txt y los introducimos en la lista de canciones 
        {
            StreamReader F;
            try
            {
                // Abro el fichero 
                F = new StreamReader("D:\\canciones.txt");//Leemos el fichero canciones.txt ubicado en D
                string linea;
                int i;
                linea = F.ReadLine();
                i = 0;
                while (linea != null)
                {
                    // Leo datos de la siguiente cancion (numero.nombre-autor) 
                    Cancion c = new Cancion();//Creamos una nueva canción en cada linea que no esté vacia
                    string[] trozos = linea.Split('.');//El primer trozo contiene tan solo el numero
                    string[] trozos1 = trozos[1].Split('-');//Del segundo trozo, al dividir la linea, hay que hacer tambien una division para separar y obtener el nombre y el autor de la cancion
                    c.numerocancion = Convert.ToInt32(trozos[0]);
                    c.nombre = trozos1[0];
                    c.autor = trozos1[1];
                    lista.canciones[i] = c;
                    i++;
                    linea = F.ReadLine();
                }
                lista.num = i;//El numero de canciones de la lista será igual a las veces que se haya entrado en el bucle
                F.Close();//Cerramos el fichero
                //si lee bien, devuelve un 0 
                return 0;

            }
            //si no encuentra el archivo devuelve un -1 
            catch (FileNotFoundException)
            {
                return -1;
            }

            //si el formato no es correcto devuelve un -2 
            catch (FormatException)
            {
                return -2;
            }
        }

        //------------------------FUNCIÓN PARA BUSCAR UNA CANCIÓN-----------------------------------------------------------------------------------------------

        static int buscarcancion(ListaCanciones lista, string nombre)
        {
            Boolean encontrado = false;
            int i = 0;

            while (i < lista.num && !encontrado)//Recorremos cada una de las canciones de la lista para ver si el nombre introducido por el usuario coincide con alguna
            {
                if (lista.canciones[i].nombre == nombre)
                {
                    encontrado = true;
                }
                else
                {
                    i++;
                }
            }
            if (!encontrado)//Si ha recorrido toda la lista y no se ha encontrado la canción, esta funcion devuelve un 0
            {
                return 0;
            }
            else//Si la cancion se ha encontrado, la funcion devuelve el numero de la cancion en la misma lista
            {
                return lista.canciones[i].numerocancion;
            }
        }

        //-----------------------REPRODUCIR CANCION-------------------------------------------------------------------------------------------------------------
        static void reproducircancion(ListaCanciones lista, String nombre)
        {
            int opcion = 0;
            SoundPlayer R = new SoundPlayer("D:\\audio\\" + nombre + ".wav"); //el archivo de audio lleva el mismo nombre de la cancion
            Console.WriteLine("Has escogido " + nombre);
            Console.Write("Introduce una opcion (1 reproducir, 2 pausar, 0 cerrar reproductor): ");
            opcion = Convert.ToInt32(Console.ReadLine());
            while (opcion != 0)
            {
                if (opcion == 1)//Al escoger la opcion 1 del menú de reproduccion, la cancion empieza a sonar
                {
                    R.Play();
                }
                else if (opcion == 2)//Al escoger la opcion 2 del menú la cancion deja de sonar
                {
                    R.Stop();
                }
                else
                {
                    Console.WriteLine("Opcion incorrecta");
                }
                Console.Write("Introduce una opcion (1 reproducir, 2 pausar, 0 cerrar reproductor): ");//Hasta que no se escoge la opcion de cerrar el reproductor, no se vuelve al menú principal
                opcion = Convert.ToInt32(Console.ReadLine());
            }
            R.Stop();//En el momento en el que cerramos el reproductor, si una canción esta sonando, tambien hay que pararla
        }

        //----------------------------------PIANO----------------------------------------------------------------------------------------------
        static void jugarPiano()
        {
            Boolean encontrado = false;
            Console.WriteLine("Hora de tocar el piano: ");
            Console.WriteLine("Instrucciones: ");
            Console.WriteLine("Pulsar las letras en teclado que corresponde a la nota+Enter para que suene, para salir pulsa la letra q+Enter");
            Console.WriteLine("DO  RE  MI  FA  SOL  LA  SI  DO* //   DO#  RE#  FA#  SOL#  LA#");
            Console.WriteLine("a   s   d   f   g    h   j   k   //   w    e    t    y     u");
            char nota;
            while (!encontrado)//Hasta que el usuario no introduzca la q para salir, el programa no volverá al menú principal
            {
                nota = Convert.ToChar(Console.Read());
                switch (nota)
                {
                    case 'a': Console.Beep(261, 600);//Suena el do
                        break;
                    case 's': Console.Beep(293, 600);//Suena el re
                        break;
                    case 'd': Console.Beep(329, 600);//Suena el mi
                        break;
                    case 'f': Console.Beep(349, 600);//Suena el fa 
                        break;
                    case 'g': Console.Beep(392, 600);//Suena el sol
                        break;
                    case 'h': Console.Beep(440, 600);//Suena el la
                        break;
                    case 'j': Console.Beep(493, 600);//Suena el si
                        break;
                    case 'k': Console.Beep(523, 600);//Suena el do alto
                        break;
                    case 'w': Console.Beep(277, 600);//Suena el do sostenido
                        break;
                    case 'e': Console.Beep(311, 600);//Suena el re sostenido
                        break;
                    case 't': Console.Beep(370, 600);//Suena el fa sostenido
                        break;
                    case 'y': Console.Beep(415, 600);//Suena el sol sostenido
                        break;
                    case 'u': Console.Beep(466, 700);//Suena el la sostenido
                        break;
                    case 'q': encontrado = true;//Si pulsa q, acaba el bucle, la función, y vuelve al menú principal
                        break;
                    default: break;
                }
            }
            Console.ReadLine();
        }

        //------------------------------------------TOCAR ALEATORIO----------------------------------------------------------------------------------------

        static void tocarAleatorio()
        {
            Random rnd = new Random();
            int cantidad = 16;
            int i = 0;
            int cont = 0;
            Boolean encontrado = false;
            while (!encontrado)//Hasta que el usuario no introduzca un numero inferior a 15 y mayor que 0 no se sale del bucle
            {
                while ((cantidad > 15) || (cantidad < 1))
                {
                    Console.Write("Cuantas notas consecutivas quieres?(Maximo 15): ");//Pedimos al usuario el numero de notas seguidas que quiere escuchar, poniendo un maximo de 15 porque es dificil recordar muchas notas
                    try
                    {
                        cantidad = Convert.ToInt32(Console.ReadLine());
                        if ((cantidad <= 15) && (cantidad >= 1))//Si la cantidad que pone el usuario es menor que 15 y el formato es correcto, sale del bucle y no pide que inserte otra vez el numero de notas
                        {
                            encontrado = true;
                        }
                    }
                    catch (FormatException)//Si el usuario introduce un dato con el formato incorrecto le sale un error
                    {
                        Console.WriteLine("ERROR: datos incorrectos");
                    }
                }
            }
            char[] aleatorias = new char[cantidad];//Vector que se forma con las notas aleatorias
            char[] tocadas = new char[cantidad];//string de las notas que hemos tocado con el piano
            while (i < cantidad)//HASTA QUE NO HAYAN SONADO TANTAS NOTAS COMO PIDE EL USUARIO NO SE SALE DEL BUCLE
            {
                int num = rnd.Next(1, 13);
                switch (num)
                {
                    case 1: Console.Beep(261, 600);//Suena el do
                        aleatorias[i] = 'a';
                        break;
                    case 2: Console.Beep(293, 600);//Suena el re
                        aleatorias[i] = 's';
                        break;
                    case 3: Console.Beep(329, 600);//Suena el mi
                        aleatorias[i] = 'd';
                        break;
                    case 4: Console.Beep(349, 600);//Suena el fa 
                        aleatorias[i] = 'f';
                        break;
                    case 5: Console.Beep(392, 600);//Suena el sol
                        aleatorias[i] = 'g';
                        break;
                    case 6: Console.Beep(440, 600);//Suena el la
                        aleatorias[i] = 'h';
                        break;
                    case 7: Console.Beep(493, 600);//Suena el si
                        aleatorias[i] = 'j';
                        break;
                    case 8: Console.Beep(523, 600);//Suena el do alto
                        aleatorias[i] = 'k';
                        break;
                    case 9: Console.Beep(277, 600);//Suena el do sostenido
                        aleatorias[i] = 'w';
                        break;
                    case 10: Console.Beep(311, 600);//Suena el re sostenido
                        aleatorias[i] = 'e';
                        break;
                    case 11: Console.Beep(370, 600);//Suena el fa sostenido
                        aleatorias[i] = 't';
                        break;
                    case 12: Console.Beep(415, 600);//Suena el sol sostenido
                        aleatorias[i] = 'y';
                        break;
                    case 13: Console.Beep(466, 700);//Suena el la sostenido
                        aleatorias[i] = 'u';
                        break;
                    default: break;
                }
                i++;
            }
            i = 0;
            Console.WriteLine("Introduce las notas que crees que han sonado anteriormente: ");
            char nota;
            while (i < cantidad)
            {
                nota = Convert.ToChar(Console.ReadLine());//Leemos las notas que el usuario introduce por teclado
                switch (nota)
                {
                    case 'a': Console.Beep(261, 600);//Suena el do
                        tocadas[i] = 'a';
                        break;
                    case 's': Console.Beep(293, 600);//Suena el re
                        tocadas[i] = 's';
                        break;
                    case 'd': Console.Beep(329, 600);//Suena el mi
                        tocadas[i] = 'd';
                        break;
                    case 'f': Console.Beep(349, 600);//Suena el fa 
                        tocadas[i] = 'f';
                        break;
                    case 'g': Console.Beep(392, 600);//Suena el sol
                        tocadas[i] = 'g';
                        break;
                    case 'h': Console.Beep(440, 600);//Suena el la
                        tocadas[i] = 'h';
                        break;
                    case 'j': Console.Beep(493, 600);//Suena el si
                        tocadas[i] = 'j';
                        break;
                    case 'k': Console.Beep(523, 600);//Suena el do alto
                        tocadas[i] = 'k';
                        break;
                    case 'w': Console.Beep(277, 600);//Suena el do sostenido
                        tocadas[i] = 'w';
                        break;
                    case 'e': Console.Beep(311, 600);//Suena el re sostenido
                        tocadas[i] = 'e';
                        break;
                    case 't': Console.Beep(370, 600);//Suena el fa sostenido
                        tocadas[i] = 't';
                        break;
                    case 'y': Console.Beep(415, 600);//Suena el sol sostenido
                        tocadas[i] = 'y';
                        break;
                    case 'u': Console.Beep(466, 700);//Suena el la sostenido
                        tocadas[i] = 'u';
                        break;
                    default: break;
                }
                i++;
            }
            i = 0;
            while (i < cantidad)//Cada vez que en la misma posicion del vector, el contenido de ambos vectores en esa posicion sea distinto, es un fallo, así que usamos un contador para saber el numero de fallos
            {
                if (tocadas[i] != aleatorias[i])
                {
                    cont++;
                }
                i++;
            }
            if (cont == 0)//Si el contador de fallos es 0, quiere decir que hemos acertado todas las notas
            {
                Console.WriteLine("Enhorabuena CRACK, las has acertado todas!!!");
            }
            else
            {
                Console.WriteLine("Has fallado {0} notas", cont);
            }
        }

        //------------------------ MAIN ------------------------------------------------------------------------------------------------------------------------

        static void Main(string[] args)
        {
            ListaCanciones lista = new ListaCanciones();
            lista.canciones = new Cancion[MAX];
            int res = cargarDatos(lista);//se leen los datos de las canciones del fichero txt
            if (res == -1)//si la funcion de cargar datos nos devuelve un -1 implica que no ha encontrado el fichero
            {
                Console.WriteLine("ERROR! Fichero no encontrado");
            }
            else if (res == -2)//Si la funcion nos devuelve un -2 es porque no ha podido leer bien los datos
            {
                Console.WriteLine("ERROR! Datos incorrectos");
            }
            else
            {
                Console.WriteLine("Datos leidos correctamente");
                int opcion = 2;//inicializamos una variable opcion a un numero distinto de 0 para que no se cierre el programa tan solo abrirlo 
                while (opcion != 0)//Mientras la opcion no sea 0 (que se acaba el programa) el programa no acaba
                {
                    Console.WriteLine("OPCIONES PRINCIPALES:");
                    Console.WriteLine("0: CERRAR PROGRAMA");
                    Console.WriteLine("1: MOSTRAR CANCIONES EN PANTALLA");
                    Console.WriteLine("2: AÑADIR UNA CANCIÓN");
                    Console.WriteLine("3: ELIMINAR UNA CANCIÓN");
                    Console.WriteLine("4: REPRODUCIR UNA CANCIÓN");
                    Console.WriteLine("5: BUSCAR CANCIÓN");//devuelve el numero si está en la lista, sino te devuelve un 0 y te dice que no esta 
                    Console.WriteLine("6: MODO PIANO");
                    Console.WriteLine("7: MODO INTERACTIVO");
                    Console.WriteLine("8: REPRODUCCIÓN ALEATORIA");
                    Console.WriteLine();
                    int error1 = 1;
                    while (error1 != 0)
                    {
                        Console.Write("ELIJE UNA OPCION:");//Pedimos al usuario que escoja una opcion
                        try
                        {
                            opcion = Convert.ToInt32(Console.ReadLine());
                            error1 = 0;
                        }
                        catch (FormatException)//En el caso que ponga otro formato al introducir la opcion, por ejemplo un string, se mostrará por pantalla opcion incorrecta y volverá a pedir una opción al usuario tantas veces como sea necesario
                        {
                            Console.WriteLine("Opcion incorrecta");
                        }
                    }
                    switch (opcion)
                    {
                        //-----------------------------------------------------------------------IMPRIMIR LISTA CANCIONES---------------------------------------------------------------------//
                        case 1: Console.WriteLine("HAS ESCOGIDO: MOSTRAR CANCIONES POR PANTALLA");
                            Console.WriteLine();
                            escribirCanciones(lista);//Esta funcion muestra por pantalla las canciones de la lista una por una
                            break;
                        //-----------------------------------------------------------------------AÑADIR CANCION-------------------------------------------------------------------------------//
                        case 2: Console.WriteLine("HAS ESCOGIDO: AÑADIR UNA CANCIÓN (RECUERDA QUE TIENES QUE AÑADIR EL ARCHIVO .WAV A LA CARPETA DE CANCIONES PARA PODER REPRODUCIRLA)");
                            Console.WriteLine();
                            Console.Write("Escribe nombre de la cancion a añadir: ");
                            string nombrecancion = Console.ReadLine();
                            int aux = buscarcancion(lista, nombrecancion);
                            if (aux == 0)//Si la funcion nos devuelve un 0 quiere decir que no la ha encontrado en la lista, por tanto podemos añadirla
                            {
                                Cancion nueva = new Cancion();//Creamos una nueva cancion y pedimos al usuario el nombre del autor para añadirla a la lista posteriormente
                                Console.Write("Escribe el nombre de su autor: ");
                                nueva.autor = Console.ReadLine();
                                nueva.numerocancion = lista.num + 1;//Como la cancion estará en la última posicion, la lista se hace 1 espacio más y el numero de cancion será el ultimo de la lista
                                nueva.nombre = nombrecancion;
                                res = ponCancion(lista, nueva);//Añadimos la cancion a la lista. Esta funcion nos devuelve un entero, un 0 en el caso que se añada y un 1 si ya esta llena
                                if (res == 0)
                                {
                                    Console.WriteLine("Añadida");
                                    Console.WriteLine();
                                }
                                else if (res == -1)
                                {
                                    Console.WriteLine("La lista ya esta llena");
                                    Console.WriteLine();
                                }

                            }
                            else//si la funcion buscar encuentra la cancion, el programa nos dice en que posición de la lista se encuentra
                            {
                                Console.WriteLine("La cancion ya esta en la lista y esta en la posicion " + aux);
                                Console.WriteLine();
                            }
                            guardarDatos(lista);//Cada vez que modificamos la lista, guardamos los datos en el fichero txt que teniamos inicialmente
                            break;
                        //------------------------------------------------------------ELIMINACION------------------------------------------------------------------------------------------------------//
                        case 3: String name = " ";
                            Boolean encontrado = false;
                            Console.WriteLine("HAS ESCOGIDO: ELIMINAR UNA CANCIÓN");
                            Console.WriteLine();
                            while (!encontrado)
                            {
                                Console.Write("Escribe el nombre de la canción que quieras eliminar o introduce salir para volver al menú:");
                                name = Console.ReadLine();
                                res = buscarcancion(lista, name);//buscamos si la cancion que ha introducido está en la lista actual de canciones
                                if (name == "salir")//si el usuario introduce salir, automaticamente sale del bucle para volver al menú principal.
                                {
                                    encontrado = true;
                                }
                                else if (res != 0)//Si la funcion buscar nos devuelve un numero distinto de 0, significa que ha encontrado la cancion y por lo tanto procedemos a su eliminacion
                                {
                                    quitaCancion(lista, res);
                                    encontrado = true;
                                    Console.WriteLine("Canción eliminada");
                                    Console.WriteLine();
                                }
                                else//Si el usuario ha escrito mal la cancion, no sale del bucle, pone que no la ha encontrado y le vuelve a preguntar el nombre.
                                {
                                    Console.WriteLine("ERROR! Canción no encontrada");
                                    Console.WriteLine();
                                }
                            }
                            guardarDatos(lista);//Cada vez que modificamos la lista, guardamos los datos en el fichero txt que teniamos inicialmente
                            break;
                        //---------------------------------------------------------------REPRODUCCION------------------------------------------------------------------------------//
                        case 4: Console.WriteLine("HAS ESCOGIDO: REPRODUCIR UNA CANCION");
                            Console.WriteLine();
                            Console.Write("Escribe el nombre de la canción que quieras reproducir:");
                            String nombre = Console.ReadLine();
                            res = buscarcancion(lista, nombre);//Buscamos en la lista actual de canciones si está la canción que queremos reproducir
                            if (res != 0)//Si la funcion nos devuelve un numero distinto de 0 indica que la ha encontrado, asi que procede a reproducirla
                            {
                                reproducircancion(lista, nombre);
                            }
                            else//Si no ha encontrado la canción en la lista, da un mensaje de error diciendo que no la ha encontrado
                            {
                                Console.WriteLine("ERROR: Cancion no encontrada");
                            }
                            break;
                        //---------------------------------------------------------------BÚSQUEDA DE CANCIÓN------------------------------------------------------------------------//
                        case 5: Console.WriteLine("HAS ESCOGIDO: BUSCAR UNA CANCION");
                            Console.WriteLine();
                            Console.Write("Escribe el nombre de la canción que quieras buscar: ");
                            nombre = Console.ReadLine();
                            res = buscarcancion(lista, nombre);//Buscamos la cancion en la lista de canciones
                            if (res != 0)//Si la funcion nos devuelve un numero distinto de 0 quiere decir que ha encontrado la cancion, y nos devuelve exactamente la posicion de la cancion en la lista
                            {
                                Console.WriteLine("Se ha encontrado la canción y está en la posición " + res);
                                Console.WriteLine();
                            }
                            else//Si la funcion nos devuelve un 0 quiere decir que no ha encontrado la cancion que buscamos
                            {
                                Console.WriteLine("Canción no encontrada en la lista");
                                Console.WriteLine();
                            }
                            break;
                        //----------------------------------------------------------------MODO PIANO----------------------------------------------------------------------------------//
                        case 6: Console.WriteLine("HAS ESCOGIDO: MODO PIANO");
                            Console.WriteLine();
                            jugarPiano();//Este modo solo permite tocar el piano, en ningun momento guardará una canción
                            break;
                        //----------------------------------------------------------------MODO INTERACTIVO----------------------------------------------------------------------------//
                        case 7: Console.WriteLine("HAS ESCOGIDO: MODO INTERACTIVO");
                            Console.WriteLine();
                            tocarAleatorio();//En este modo suenan notas aleatorias y posteriormente tienes que acertarlas
                            break;
                        //----------------------------------------------------------------REPRODUCCION ALEATORIA DE LAS CANCIONES DE LA LISTA-----------------------------------------//
                        case 8: Console.WriteLine("HAS ESCOGIDO: REPRODUCCION ALEATORIA");
                            Console.WriteLine();
                            int option = -1;
                            int error = 1;//asumimos que hay un error
                            while (error == 1)//mientras haya un error, no saldremos del bucle
                            {
                                Console.Write("Pulsa 1 si quieres escuchar una canción aleatoria y 0 si quieres salir al menú principal: ");//si el usuario introduce un 1, se crea un numero al azar y se ejecuta la canción cuya posición en el vector es esa
                                try
                                {
                                    option = Convert.ToInt32(Console.ReadLine());//Pedimos una opcion al usuario
                                    Console.WriteLine();//Si los datos estan bien introducidos, no hay error, por tanto la variable error pasa a ser nula
                                    error = 0;
                                }
                                catch (FormatException)//si el usuario no escribe un numero, le salta un error de formato y le volvemos a pedir que introduzca una opcion
                                {
                                    Console.WriteLine("ERROR!!! DATOS INCORRECTOS");
                                    Console.WriteLine();
                                    error = 1;
                                }
                                if (option != 1 && option != 0)//si la opcion no es ni 1 ni 0, volvemos a pedir al usuario que introduzca una opción valida ( para eso cambiamos el valor de la variable error para que no salga del bucle )
                                {
                                    error = 1;
                                    Console.WriteLine("ERROR!!!");
                                    Console.WriteLine();
                                }
                            }

                            while (option != 0)
                            {
                                error = 1;
                                Random rnd = new Random();//creamos una variable que almacenará el numero al azar
                                int song = rnd.Next(0, lista.num - 1);//Obtenemos un numero al azar entre el 0 y el numero de canciones que hay en la lista -1 (debido a que va por posiciones)
                                reproducircancion(lista, lista.canciones[song].nombre);//Reproducimos la cancion que está en la posición del vector que hemos obtenido aleatoriamente
                                Console.WriteLine("HAS SALIDO DEL MENÚ REPRODUCCIÓN VUELVES A ESTAR EN EL MENÚ DE REPRODUCCIÓN ALEATORIA");
                                Console.WriteLine();
                                while (error == 1)//volvemos al inicio, como si acabasemos de escoger la opcion del menú principal de reproducción aleatoria
                                {
                                    Console.Write("Pulsa 1 si quieres escuchar una canción aleatoria y 0 si quieres salir al menú principal: ");
                                    try//si el usuario introduce los datos correctamente, no hay error, con lo cual la variable error pasa a ser nula
                                    {
                                        option = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine();
                                        error = 0;
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("ERROR!!! DATOS INCORRECTOS");//si el usuario no escribe un numero, salta un error porque los datos son incorrectos y vuelve a pedir que introduzca una opcion
                                        error = 1;
                                    }
                                    if (option != 1 && option != 0)//si la opcion no es ni 1 ni 0, volvemos a pedir al usuario que introduzca una opción valida ( para eso cambiamos el valor de la variable error para que no salga del bucle )
                                    {
                                        error = 1;
                                        Console.WriteLine("ERROR!!!");
                                        Console.WriteLine();
                                    }
                                }
                            }
                            break;
                        //----------------------------------------------------------------SALIR DEL PROGRAMA--------------------------------------------------------------------------//
                        case 0: break;
                        //----------------------------------------------------------------POR DEFECTO---------------------------------------------------------------------------------//
                        default: break;
                    }
                }
                Console.WriteLine("ADIOS! HASTA LA PROXIMA!!");
            }
            Console.ReadLine();
        }
    }
}





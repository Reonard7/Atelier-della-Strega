05/12 - Leonardo
Ho aggiunto pozioni e ingredienti tramite ScriptableObjects. Sono molto semplici da creare: se si vuole creare un nuovo tipo di SO, basta
andare nella cartella GameData > Scripts > Items, qui fare tasto dx > Create > Scripting > New SO. Funziona come un classico script, che
vuole delle variabili (vedasi esempi di altri oggetti).

Se si vuole inizializzare un SO già definito, andare nella cratella dove infilare il SO, tasto dx > Create > GameData (è in basso).
Una volta creato, si crea un file .asset, che rappresenta l'entità di gioco (ingrediente, pozione, ecc.), in cui nell'ispector si possono
dare le diverse variabili (nome, id).


07/12 - Leonardo
Finalmente qualche interazione. Per il player ho copia-incollato uno script di Strada (che non funziona proprio benissimo, non salta e
il movimento è laggoso), idem per gli script di interazione. In poche parole, ho creato un Object con uno script IngredientInteractable:
questo eredita a sua volta dallo script Interactable (che ha solo la definizione di un metodo da chiamare se ci si interagisce col sx); la
cosa interessante di IngredientInteractable è che ha una variabile per uno ScriptableObject, quindi basta attaccare lo SO Ingredient che
desideriamo e da quel momento quell'oggetto sarà riconosciuto come Ingredient.

L'obiettivo adesos è popolare la scena di tanti ingredienti, e vedere se il testo a schermo funzioni correttamente; poi, c'è da creare un
IngredientManager che all'avvio tenga in storage tutti gli ingredienti in scena, così possiamo fare cose utili tipo far sparire l'ingrediente
se messo nell'inventario o farlo ritornare al suo posto.
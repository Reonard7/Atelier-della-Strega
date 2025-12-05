05/12 - Leonardo
Ho aggiunto pozioni e ingredienti tramite ScriptableObjects. Sono molto semplici da creare: se si vuole creare un nuovo tipo di SO, basta
andare nella cartella GameData > Scripts > Items, qui fare tasto dx > Create > Scripting > New SO. Funziona come un classico script, che
vuole delle variabili (vedasi esempi di altri oggetti).

Se si vuole inizializzare un SO già definito, andare nella cratella dove infilare il SO, tasto dx > Create > GameData (è in basso).
Una volta creato, si crea un file .asset, che rappresenta l'entità di gioco (ingrediente, pozione, ecc.), in cui nell'ispector si possono
dare le diverse variabili (nome, id).
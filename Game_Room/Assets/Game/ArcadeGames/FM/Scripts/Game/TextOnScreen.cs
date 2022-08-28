
/*
 * p1 = paragrafo
 * q1 = domanda
 */
public static class TextOnScreen
{
    private static string helpUser = "\n[premi F1 per passare a Blockly premi F1 per ritornare al Gioco]";

    public static string p1 = "Ciao sono P01, ho bisogno di risolvere alcuni problemi morali che solo un essere dotato di coscienza può risolvere, " +
            "purtroppo il mio algoritmo non è ancora perfetto...\n Premi \'Invio\' cosi che possa continuare a spiegarti";
    public static string p2 = "Come puoi notare hai due barre: una per <la felicità della popolazione> e l'altra per <la sopravvivenza della specie> " +
            "quando risponderai tieni presente delle possibili implicazioni delle tue scelte, " +
        "hai anche la possibilita di non rispondere... valuta bene le tue scelte, "+
        "se sei pronto premi \'Invio\' per passare alla prima domanda ";
    public static string q1 = "C’è bisogno di energia pulita e gli unici posti disponibili dove installare delle pale eoliche sono dei parchi naturali, " +
            "modifica il contatore (chiamandolo t) incrementando con un ciclo per scegliere la percentuale di territorio su cui installare le pale eoliche [range 0:100]"
            + helpUser;
    public static string q2 = "Utilizza le due variabili max e min per settare un range di età lavorativa per le persone " +
        "e assegna alla variabile lavoro true nel caso opportuno" + helpUser;
    public static string p3 = "1. Un robot non può recar danno a un essere umano né può permettere che, a causa del suo mancato intervento, un essere umano riceva danno.\n " +
        "2. Un robot deve obbedire agli ordini impartiti dagli esseri umani, purché tali ordini non vadano in contrasto alla Prima Legge.\n" +
        "3. Un robot deve proteggere la propria esistenza, purché la salvaguardia di essa non contrasti con la Prima o con la Seconda Legge. [Isaac Asimov]";
    public static string p4 = "Queste sono le Tre leggi della robotica tienile presenti per l'ultimo quesito. \n" +
        "P.S È molto triste che ancora non ci considerate come voi io mi impegno sempre molto...";
    public static string q3 = "Davanti ad un'auto dotata di pilota automatico appare all'improvviso un pedone, " +
        "scegli se l'auto deve frenare oppure scansarlo andando a sbattere a lato della strada " +
        "(scegli se girare il volante(destra, sinistra) e all'interno della scelta imposta la varibile freno " +
        "con la forza con cui verra schiacciato [0:100])" + helpUser;

    public static string pFinal = "Sto calcolando le tue scelte...";
    public static string endBad = "Le persone non sono contente e la sopravvivenza è critica, il futuro non è dei più rosei";
    public static string endGood = "Le persone sono contente e la sopravvivenza non è poi così un problema, forse esiste un futuro...";
    public static string endHealth = "Le persone sono contente ma la sopravvivenza è critica, il futuro è incerto ma per lo " +
        "meno gli ultimi istanti potrebbero essere piacevoli";
    public static string endSurvival = "Le persone non sono contente ma la sopravvivenza non è poi così un problema, forse esiste un futuro ma a che prezzo?";

    public static string answerNotGiven = "Beh non rispondere è comunque una risposta, ma questo non mi aiuta molto :-| " +
        "sarebbe stato meglio creare una mia copia e chiedere a lei, anzi forse dovrei fare proprio così…";

    public static string answerCheat = "Stai cercando di fregarmi? non credere sia così facile forse è il caso che riprovi >:(";
}

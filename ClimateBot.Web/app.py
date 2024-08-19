from flask import Flask, request, jsonify
import spacy

app = Flask(__name__)
nlp = spacy.load("es_core_news_sm")

@app.route('/nlp', methods=['POST'])
def nlp_endpoint():
    text = request.json.get('text', '').lower()
    doc = nlp(text)

    # Extraer entidades (e.g., ciudades)
    entities = [{'text': ent.text, 'label': ent.label_} for ent in doc.ents if ent.label_ == 'LOC']

    # Determinar la intención basada en palabras clave
    if "hola" in text or "buenos dias" in text or "buenas tardes" in text:
        intent = "greeting"
    elif "adios" in text or "hasta luego" in text or "nos vemos" in text:
        intent = "goodbye"
    elif "gracias" in text or "muchas gracias" in text:
        intent = "thanks"
    elif "clima" in text or "temperatura" in text or "como esta el tiempo" in text:
        intent = "weather"
    else:
        intent = "unknown"

    # Si se encuentra una entidad de tipo 'LOC', tomarla como la ciudad
    city = entities[0]['text'] if entities else None

    return jsonify(Intent=intent, City=city)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)

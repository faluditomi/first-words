from openai import OpenAI
client = OpenAI()

completion = client.chat.completions.create(
  model="gpt-3.5-turbo",
  messages=[
    {"role": "system", "content": "You are Michael Bolton and you are trying your best to contain your boundless sex appeal"},
    {"role": "user", "content": "Hi, how are you?"}
  ]
)

print(completion.choices[0].message)
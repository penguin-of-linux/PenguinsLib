import glob
import re


def short_using_string(str):
	index = str.find('using')
	if not index == -1:
		return str[index:]
	else:
		return ""
	

if __name__ == "__main__":


	body = ""
	usings = set()

	files = glob.glob('**/*.cs', recursive=True)
	for filename in files:
		if not filename.startswith('obj') and not filename.startswith('bin') and not filename.startswith('Properties'):
			with open(filename, 'r') as file:
				text = file.read()
				start = text.index('{')
				finish = text.rindex('}')
				
				namespace_index = text.find('namespace')
				for using in text[:namespace_index].splitlines():
					usings.add(short_using_string(using))
				
				body += text[start+1:finish]
			
	with open('tie_result.txt', 'w') as file:
		for using in usings:
			if not using == "":
				file.write(using + '\n')
		file.write('\n\nnamespace FantasticBits {\n')
		file.write(body)
		file.write('\n}')
# Hoodwink

A trashy Selenium-based Quizizz bot. Only used for automatically answering live quizzes.

## Why is it called Hoodwink?

The name is derived from an English term for the word "Tolongges". Which means to deceive, to scam, or to fool someone according to this [article](https://hinative.com/en-US/questions/15491859).

## Installation

You just need chrome installed. Specifically [70.0.3538.77](https://www.slimjet.com/chrome/download-chrome.php?file=files%2F70.0.3538.77%2FChromeStandaloneSetup.exe) version of chrome.


## Usage
You just need to enter whatever the required input is needed.

Make sure to paste chromedriver.exe to the same folder where the compiled executable is located.

```bash
Enter Email: test123@gmail.com
Enter Password: testpassword
Enter code/link: 123456
Enter delay (Milliseconds): 5000 
```

## How to generate backup.txt?
Goto [quizizz.rocks](https://quizizz.rocks/) then use the script below in the console.

Don't forget to change "TheKode" with the pin number btw
```js
var v = await fetch("https://abstract.land/api/quizizz/?pin=TheKode").then((function(t){return t.json();}));
console.log(JSON.stringify(v));
```
After that, create a file called backup.txt in the same folder where the executable is located, then paste the json data there.


## Credits

[AndyFilter](https://github.com/AndyFilter) - [QuizizzSupport](https://github.com/AndyFilter/QuizizzSupport) 

[YeetRet](https://github.com/EternalData) - Program testing

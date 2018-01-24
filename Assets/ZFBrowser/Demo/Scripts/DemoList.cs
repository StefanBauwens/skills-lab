using System.Collections.Generic;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser {
[RequireComponent(typeof(Browser))]
public class DemoList : MonoBehaviour {
	protected List<string> demoSites = new List<string> {
		"localGame://demo/MouseShow.html",//simple, cheap circle follows mouse, fade
		"http://js1k.com/2013-spring/demo/1487",//kalidescope effect around mouse

//		"http://js1k.com/2013-spring/demo/1471",//black balls follow mouse

		"http://js1k.com/2014-dragons/demo/1868", //webgl blobs
		"http://glimr.rubyforge.org/cake/missile_fleet.html",//spaceships shoot each other
		"http://js1k.com/2015-hypetrain/demo/2231", //galaxy
		"http://js1k.com/2015-hypetrain/demo/2313",//particles, music

		"http://js1k.com/2015-hypetrain/demo/2331", //wave simulator in a dot grid
		"http://js1k.com/2015-hypetrain/demo/2315",//drag starfield
		"http://js1k.com/2015-hypetrain/demo/2161", //animated 3d fractal

		"http://js1k.com/2013-spring/demo/1533", //raindrop noise/music
		"http://js1k.com/2014-dragons/demo/1969",//many cube lines

		"http://www.snappymaria.com/misc/TouchEventTest.html",//circle around mouse cursor
//		"http://js1k.com/2013-spring/demo/1456",//plasma
//		"http://js1k.com/2013-spring/demo/1511",//circles around the mouse cursor
	};

	public Browser demoBrowser;
	private Browser panelBrowser;

	private int currentIndex = 0;

	protected void Start() {
		panelBrowser = GetComponent<Browser>();
		panelBrowser.RegisterFunction("go", args => {
			DemoNav(args[0].Check());
		});

		demoBrowser.onLoad += info => {
			panelBrowser.CallFunction("setDisplayedUrl", demoBrowser.Url);
		};

		demoBrowser.Url = demoSites[0];
	}

	private void DemoNav(int dir) {
		if (dir > 0) {
			currentIndex = (currentIndex + 1) % demoSites.Count;
		} else {
			currentIndex = (currentIndex - 1 + demoSites.Count) % demoSites.Count;
		}

		demoBrowser.Url = demoSites[currentIndex];
	}
}
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public static bool UIHover;

	private GameObject selection;
	private GameObject time;
	private GameObject restitution;
	
	// This is a list of all the classes that should show up in the UI.
	private static List<Type> classes = new List<Type>();

	public GUISkin skin;

	// This method adds the classes to the list.
	public static void registerClass<T>()
	{
		classes.Add(typeof(T));
	}

	void OnGUI()
	{
		GUI.skin = skin;
	}
	
	// Use this for initialization
	void Start ()
	{
		foreach(Transform t in transform)
		{
			switch(t.name)
			{
					case "Selection":
						selection = t.gameObject;
						// Makes sure the inspector window is disabled by default.
						selection.GetComponent<Image>().enabled = false;
						break;
					case "Time":
						// Sets the time variable to the gameobject to be used later.
						time = t.gameObject;
						break;
					case "SaveLoad":
						// Loads all the games from the file and puts them in SaveLoad.savedGames.
						SaveLoad.Load();
						// Gets the Content UI element which is a great grandchild of t.
						Transform d = t.Find("Scroll View").Find("Viewport").Find("Content");
						// Loops through each saved game.
						for(int i = 0; i < SaveLoad.savedGames.Count; i++)
						{
							// Creates a new button and casts it (safely) to a GameObject.
							GameObject g = Instantiate(Resources.Load("Button")) as GameObject;
							// Sets the parent to the Content UI element.
							g.transform.parent = d;
							// Sets the position based on its index.
							RectTransform pos = g.GetComponent<RectTransform>();
							pos.localPosition = new Vector3(0, -(15 + i * 30), 0);
							// This fixes a bug in Unity's implementation of Mono
							// that means that you cannot use variables from outer scopes
							// in delegates.
							int j = i;
							// Gets the button script from the current button.
							Button b = g.GetComponent<Button>();
							// Sets the text to the index.
							b.GetComponentInChildren<Text>().text = i.ToString();
							// When the button is clicked it will load the required game.
							b.onClick.AddListener(delegate
							{
								LoadGame(j);
							});
						}
						
						// Finds the save button and sets it so when its clicked
						// it will save the current game.
						Button button = t.FindChild("Save").GetComponent<Button>();
						Debug.Log(button);
						button.onClick.AddListener(() =>
							SaveGame());
						break;
			}
		}

		foreach(Transform t in time.transform)
		{
			switch(t.name)
			{
				case "Play":
					// When the button is clicked it will fire the onPlayClick event.
					t.GetComponent<Button>().onClick.AddListener(() => { onPlayClick(t); });
					break;
				case "Speed":
					InputField input = t.GetComponentInChildren<InputField>();
					// Sets the game speed to the value in the text box when changed.
					input.onValueChanged.AddListener(value =>
					{
						Master.INSTANCE.speed = float.Parse(value);
					});
					input.onEndEdit.AddListener(value =>
					{
						input.text = Master.INSTANCE.speed.ToString();
					});
					input.text = "1.0";
					break;
				case "Restitution":
					InputField i = t.GetComponentInChildren<InputField>();
					i.onValueChanged.AddListener(value =>
					{
						Col.E = float.Parse(value);
					});
					i.onEndEdit.AddListener(value =>
					{
						i.text = Col.E.ToString();
					});
					i.text = "1.0";
					break;
				case "Add Ball":
					Button button = t.GetComponent<Button>();
					button.onClick.AddListener(() =>
					{
						// Creates a normal particle.
						GameObject g = (GameObject)Instantiate(Resources.Load("Object"));
						RaycastHit info;
						// Raycasts forward from the camera by 10.
						Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) / 2);
						if(Physics.Raycast(ray, out info, 10))
						{
							// If it hits, set the location of the particle to the hit point...
							g.transform.position = info.point + info.normal.normalized * 0.5f;
						}
						else
						{
							// ... otherwise place it 2 units in front of the camera.
							g.transform.position = ray.origin + ray.direction * 2;
						}
						if(Master.INSTANCE.isPlaying)
						{
							// Sets all the arrows on the object to the correct state...
							foreach(Transform a in g.transform)
							{
								Arrow arrow = a.GetComponent<Arrow>();
								if(arrow != null)
								{
									arrow.onPlayPause(false);
								}
							}
						}
						else
						{
							//... and do the same with the trail.
							TrailRenderer trailRenderer = g.GetComponent<TrailRenderer>();
							trailRenderer.enabled = false;
							trailRenderer.Clear();
						}
					});
					break;
				case "Add Plane":
					Button b = t.GetComponent<Button>();
					b.onClick.AddListener(() =>
					{
						Instantiate(Resources.Load("Plane"));
					});
					break;
				case "Trail":
					Toggle toggle = t.GetComponent<Toggle>();
					toggle.onValueChanged.AddListener(trail =>
					{
						Master.INSTANCE.Trail = trail;
					});
					break;
			}
		}
	}

	public void onPlayClick(Transform t)
	{
		Master.INSTANCE.isPlaying = !Master.INSTANCE.isPlaying;
		if(Master.INSTANCE.isPlaying)
		{
			t.GetComponentInChildren<Text>().text = "Pause";
		}
		else
		{
			t.GetComponentInChildren<Text>().text = "Play";
		}
		
	}

	void Update()
	{
		if(UI.UIHover) return;
		Selectable selected = Master.INSTANCE.Selected;
		// Resets the selection UI.
		foreach(Transform child in selection.transform) Destroy(child.gameObject);
		// If nothing is selected, then get turn off the selection pane and return.
		if(selected == null)
		{
			selection.GetComponent<Image>().enabled = false;
			return;
		}
		// Makes sure the selection pane is turned on if something is selected.
		selection.GetComponent<Image>().enabled = true;
		// This is the y coordinate of each component, it increases by 10 between each class
		// and 22 between each component in the class.
		int y = 0;
		// Loops through each class registered for the UI.
		foreach(Type clazz in classes)
		{
			y += 10;
			// Gets the instance of the current class in the selected object.
			var component = selected.GetComponent(clazz);
			if(component == null)
				component = selected.GetComponentInChildren(clazz);
			// If the selected object actually has that class.
			if(component != null)
			{
				// Gets each property with the show value in that class.
				PropertyInfo[] values = ShowValue.getValues(clazz);
				foreach(PropertyInfo value in values)
				{
					y += 22;
					drawValues(value, component, y);
				}
			}			
		}
	}

	private void drawValues(PropertyInfo value, object selected, int yPos)
	{
		Type t = value.PropertyType;
		GameObject g = null;
		if(t == typeof(int))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), selection.transform);
			InputField input = g.GetComponentInChildren<InputField>();
			input.contentType = InputField.ContentType.IntegerNumber;
			input.onValueChanged.AddListener(val =>
			{
				int x;
				if(val != null && Int32.TryParse(val, out x)) value.SetValue(selected, x, null);
			});
			input.onEndEdit.AddListener(val =>
			{
				input.text = value.GetValue(selected, null).ToString();
			});
			input.text = value.GetValue(selected, null).ToString();
		}
		else if(t == typeof(float))
		{
			g = (GameObject) Instantiate(Resources.Load("Number"), selection.transform);
			InputField input = g.GetComponentInChildren<InputField>();
			input.contentType = InputField.ContentType.DecimalNumber;
			input.onValueChanged.AddListener(val =>
			{
				float x;
				if(val != null && float.TryParse(val, out x)) value.SetValue(selected, x, null);
			});
			input.onEndEdit.AddListener(val =>
			{
				input.text = value.GetValue(selected, null).ToString();
			});
			input.text = value.GetValue(selected, null).ToString();
		}
		else if(t == typeof(Vector3))
		{
			g = (GameObject) Instantiate(Resources.Load("Vector3"), selection.transform);
			InputField x,y,z;
			x = y = z = null;
			foreach(Transform transform in g.transform)
			{
				switch(transform.name)
				{
					case "X":
						x = transform.GetComponent<InputField>();
						break;
					case "Y":
						y = transform.GetComponent<InputField>();
						break;
					case "Z":
						z = transform.GetComponent<InputField>();
						break;
				}
			}
			
			if(x == null || y == null || z == null) return;
			
			x.onValueChanged.AddListener(val =>
			{
				float x1;
				if(val == null || !float.TryParse(val, out x1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.x = x1;
				value.SetValue(selected, v, null);
			});
			
			y.onValueChanged.AddListener(val =>
			{
				float y1;
				if(val == null || !float.TryParse(val, out y1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.y = y1;
				value.SetValue(selected, v, null);
			});
			
			z.onValueChanged.AddListener(val =>
			{
				float z1;
				if(val == null || !float.TryParse(val, out z1)) return;
				Vector3 v = (Vector3) value.GetValue(selected, null);
				v.z = z1;
				value.SetValue(selected, v, null);
			});

			x.text = ((Vector3) value.GetValue(selected, null)).x.ToString();
			y.text = ((Vector3) value.GetValue(selected, null)).y.ToString();
			z.text = ((Vector3) value.GetValue(selected, null)).z.ToString();
		}
		else if(t == typeof(bool))
		{
			g = (GameObject) Instantiate(Resources.Load("Bool"));
			Toggle input = g.GetComponentInChildren<Toggle>();
			input.onValueChanged.AddListener(val =>
			{
				value.SetValue(selected, val, null);
			});
			input.isOn = (bool) value.GetValue(selected, null);
		}

		if(g == null) return;
		
		// Sets the label for the current value, since the label
		// has the tag "Tag".
		foreach(Transform tf in  g.transform)
		{
			if(tf.CompareTag("Tag"))
			{
				tf.GetComponent<Text>().text = value.Name;
			}
		}

		g.transform.position += new Vector3(0, -yPos, 0);
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		UIHover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIHover = false;
	}
	
	public void SaveGame()
	{
		Game g = Game.current = new Game();
		Base[] b = GameObject.FindObjectsOfType<Base>();
		g.particles = new Particle[b.Length];
		for(int i = 0; i < b.Length; i++)
		{
			// Creates a BaseObj object for each particle and
			// sets its position, mass, force, velocity, coefficient of friction.
			Base bse = b[i];
			g.particles[i] = new Particle(bse.Position,
				bse.Mass,
				bse.Force,
				bse.Velocity,
				bse.gameObject
					.GetComponentInChildren<Friction>()
					.Coefficient
			);
		}
		// Does the same with the planes, except only setting
		// the position and rotation.
		PlaneBase[] p = GameObject.FindObjectsOfType<PlaneBase>();
		g.planes = new PlaneObj[b.Length];
		for(int i = 0; i < p.Length; i++)
		{
			PlaneBase pb = p[i];
			g.planes[i] = new PlaneObj(pb.Position, pb.Rotation);
		}
		// Puts it into the file.
		SaveLoad.Save();
	}

	public void LoadGame(int value)
	{
		// Destroys all the planes and particles in the current scene.
		foreach(Selectable obj in FindObjectsOfType<Selectable>())
		{
			Destroy(obj.gameObject);
		}
		// Gets the required game.
		Game game = SaveLoad.savedGames[value];
		foreach(Particle p in game.particles)
		{
			// Creates a blank particle.
			GameObject g = GameObject.Instantiate(Resources.Load("Object")) as GameObject;
			// Gets the base and friction scripts.
			Base b = g.GetComponent<Base>();
			Friction f = g.GetComponentInChildren<Friction>();
			
			// Calls the starting method and sets the
			// position, coefficient, mass, force, velocity.
			b.Init();
			b.Position = p.position.toVector();
			f.Coefficient = p.coefficient;
			b.Mass = p.mass;
			b.Force = p.force.toVector();
			b.Velocity = p.velocity.toVector();
			
		}
		// Do the same for all the planes.
		foreach(PlaneObj plane in game.planes)
		{
			GameObject g = GameObject.Instantiate(Resources.Load("Plane")) as GameObject;
			PlaneBase b = g.GetComponent<PlaneBase>();
			b.Rotation = plane.rotation.toVector();
			b.Position = plane.position.toVector();
			
		}
		// Sets the current game to the new game.
		Game.current = game;
	}
}

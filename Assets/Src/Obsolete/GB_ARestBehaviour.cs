using System;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace GBAssets.Network
{
	//Jede Entität hat seine eigenen ResponseData mit Parser-Funktion die aus XML-String C# Daten macht
	public abstract class ResponseData
	{
		public abstract void Parse(string text);
	}

	//Hier die Abstrakte RestFull Behaviour
	public abstract class GB_ARestBehaviour<T> : MonoBehaviour
		 where T : ResponseData
	{
		//Die beiden Actions wie bisher
		public Action<T> DoAddData { get; private set; }
		public Action<string, T> DoRequest { get; private set; }

		//Response buffer
		protected List<T> updates = new List<T>();

		//Funktionen den Actions zuweisen
		public GB_ARestBehaviour()
			: base()
		{
			DoAddData = AddData;
			DoRequest = Request;
		}

		//Funktion zum Buffern der Responses
		private void AddData(T data)
		{
			lock (updates)
			{
				updates.Add(data);
			}
		}

		//Die private Request funktion die die BackEndStub praktisch ersetzt.
		private void Request(string query, T responseData)
		{
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(query);

			//Da die Request-funktion bereits Asynchron aufgerufen wurde, müssen wir NICHT nochmal ein Async erzeugen!
			request.Timeout = 10000;
			request.GetResponse();

			if (request.HaveResponse)
			{
				StreamReader stream = new StreamReader(request.GetRequestStream());
				string responseText = stream.ReadToEnd();

				//die responseData soll den string interpetieren
				responseData.Parse(responseText);
				//Übertrage die geparste ResponseData vom Request-Thread in den UnityThread mit Invoke!!
				DoAddData.Invoke(responseData);
				stream.Close();
			}
			else
			{
				//ERROR
				//Behandle den Fehlerfall Falls nix zurück kommt.
				//Nicht vergessen: NUR Actions dürfen ausgeführt werden.
				responseData.Parse("<error>Request Timeout!</error>");
				DoAddData.Invoke(responseData);
			}

		}
	}


	//Nun ein Anwendungsbeispiel

	//Zuerst das DataSet mit seiner Parser-Funktion definieren.
	public class ExampleData : ResponseData
	{
		public string OriginalText { get; private set; }
		public int HP { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		public override void Parse(string text)
		{
			OriginalText = text;
			//Parse den scheiß
			HP = 124;
			X = 12;
			Y = 4;
		}
	}

	//Nun die eigentliche Behaviour welche selbstständig ihre Anfragen an den Server sendet, sobald ein entsprechendes Ereignis eintritt
	public class Example : GB_ARestBehaviour<ExampleData>
	{
		public int HP { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		private void FixedUpdate()
		{
			//Setze die Updates aus den Anfragen der vorherigen Loop um.
			lock (updates)
			{
				foreach(ExampleData data in updates)
				{
					HP = data.HP;
					X = data.X;
					Y = data.Y;
				}
			}

			if (IsSomeSpecialEvent1())
			{
				//Bilde deine Anfrage je nach Ereignis
				string query = "http://server/?someAction1=true";
				AsyncCallback callback = new AsyncCallback(OnResponse);
				DoRequest.BeginInvoke(query, new ExampleData(), callback, this);
			} 
			else if (IsSomeSpecialEvent2())
			{
				//Bilde deine Anfrage je nach Ereignis
				string query = "http://server/?someAction2=true";
				AsyncCallback callback = new AsyncCallback(OnResponse);
				DoRequest.BeginInvoke(query, new ExampleData(), callback, this);
			}
		}

		private bool IsSomeSpecialEvent1()
		{
			//Prüfe ob ein bestimmtest Ereignis eingetreten ist etc...
			return true;
		}

		private bool IsSomeSpecialEvent2()
		{
			//Prüfe ob ein anderes Ereignis eingetreten ist etc...
			return true;
		}

		private void OnResponse(IAsyncResult result)
		{
			//Paar Debug infos
		}
	}
}

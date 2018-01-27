using System;


class Unititled{
	static void Main(string[]args){
		TaskManager tm = new TaskManager();

		tm.Do(()=>(Console.WriteLine("Hello"))).Then
		(new WaitTask(1000)).Then
		(new ActionTask(()=>(Console.WriteLine("Goodbye"))));

		while(true){
			tm.Update();
		}

	}

}
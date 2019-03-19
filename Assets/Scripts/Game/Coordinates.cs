using UnityEngine;
using System.Collections;

//Coordinates class for holding location and heuristic information
public class Coordinates {

	public int x;
	public float y;
	public int z;
	public int f;
	public int h;
	public int g;
	public Coordinates parent;

	public Coordinates(int x, float y, int z, int h, int g, int f, Coordinates parent) {
		this.x=x;
		this.y=y;
		this.z=z;
		this.f=f;
		this.h=h;
		this.g=g;
		this.parent = parent;
	}
	public Coordinates(Coordinates copy) {
		this.x=copy.x;
		this.y=copy.y;
		this.z=copy.z;
		this.f=copy.f;
		this.h=copy.h;
		this.g=copy.g;
		this.parent = copy.parent;
	}
}

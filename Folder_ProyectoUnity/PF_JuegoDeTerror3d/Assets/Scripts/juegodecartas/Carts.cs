using UnityEngine;
public enum Palo { Corazon, Trebol, Espada, Rubi }

public class Carts : MonoBehaviour
{
    public Palo palo;
    public int valor;
    public Material material;

    public Carts(Palo palo, int valor, Material material)
    {
        this.palo = palo;
        this.valor = valor;
        this.material = material;
    }
}

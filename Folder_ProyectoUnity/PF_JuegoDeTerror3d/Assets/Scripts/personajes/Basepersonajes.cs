using System.Threading;
using UnityEngine;

public class Basepersonajes//-> Reformular 
{
    protected int vida = 10;
    protected int velocidad = 10;
    protected int velocidadOriginal = 10;
    protected float temporizador = 0f;
    protected bool detenido = false;

    protected bool reparar = false;
    protected float temporizadorREP = 0;

    public virtual void RecibirDaño(int daño)
    {
        vida -= daño;
        velocidad -= daño;

        if (vida < 0) vida = 0;
        if (velocidad < 0) velocidad = 0;

    }
    
    public virtual void Disparado(int daño)//->remuval por linterna
    {
        if (daño > 0)
        {
            vida -= daño;
            if (vida < 0) vida = 0;

            velocidad = 0;
            detenido = true;
            temporizador = 0f;
        }
    }
    public virtual void Reparar() {//->Pilas

        reparar = true;

    }

    protected virtual void Update()
    {
        if (detenido)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= 5f)
            {
                velocidad = velocidadOriginal;
                detenido = false;
                temporizador = 0f;
            }
        }
        if (reparar) {
            temporizador += Time.deltaTime;

            if (temporizadorREP >= 8f) {
                reparar = false;
                temporizadorREP = 0f;
            }
        }
    }
}

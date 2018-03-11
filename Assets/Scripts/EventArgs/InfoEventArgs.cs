using System;

public class InfoEventArgs<T> : EventArgs
{
    public T info;

    /// <summary>
    /// Default constructor
    /// </summary>
    public InfoEventArgs()
    {
        this.info = default(T);
    }

    /// <summary>
    /// Constructor with defined infos
    /// </summary>
    /// <param name="info"></param>
    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}

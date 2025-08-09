using Godot;
using System;

public partial class ContextMessagePopup : Node2D
{

    [Export] public float MessageScrollSpeed;
    [Export] public float MessageScrollDuration;

    private RichTextLabel _label;
    private float _timeElapsed;


    public void SetText(string text)
    {
        if (_label == null) _label = GetNode<RichTextLabel>("ContextMessageBox");
        _label.Text = text;
    }

        public override void _Process(double delta)
    {
        _timeElapsed += (float)delta;
        Position += new Vector2(0, -MessageScrollSpeed * (float)delta);

        Modulate = new Color(1, 1, 1, 1f - (_timeElapsed / MessageScrollDuration));

        if (_timeElapsed >= MessageScrollDuration)
            QueueFree();
    }



}

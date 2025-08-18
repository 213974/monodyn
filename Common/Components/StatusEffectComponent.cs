// Common/Components/StatusEffectComponent.cs
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class StatusEffectComponent : Node
{
    // A helper class to track an active effect
    private class ActiveEffect
    {
        public StatusEffectResource Data;
        public float TimeLeft;
        public float TimeSinceLastTick;
    }

    private List<ActiveEffect> _activeEffects = new List<ActiveEffect>();
    private HealthComponent _healthComponent;

    public override void _Ready()
    {
        _healthComponent = GetParent<Node>().GetNode<HealthComponent>("HealthComponent");
    }

    public override void _Process(double delta)
    {
        if (_activeEffects.Count == 0) return;

        // Process all active effects
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = _activeEffects[i];
            effect.TimeLeft -= (float)delta;
            effect.TimeSinceLastTick += (float)delta;

            // If it's time for a tick
            if (effect.TimeSinceLastTick >= effect.Data.TickInterval)
            {
                effect.TimeSinceLastTick -= effect.Data.TickInterval;
                ApplyTickDamage(effect.Data);
            }

            // If the effect has expired, remove it
            if (effect.TimeLeft <= 0)
            {
                _activeEffects.RemoveAt(i);
            }
        }
    }

    public void ApplyEffect(StatusEffectResource effectData)
    {
        // If this effect type is already active, just refresh its duration
        var existingEffect = _activeEffects.FirstOrDefault(e => e.Data.Type == effectData.Type);
        if (existingEffect != null)
        {
            existingEffect.TimeLeft = effectData.Duration;
        }
        else // Otherwise, add it as a new effect
        {
            _activeEffects.Add(new ActiveEffect
            {
                Data = effectData,
                TimeLeft = effectData.Duration,
                TimeSinceLastTick = 0 // Tick immediately on apply
            });
        }
    }

    private void ApplyTickDamage(StatusEffectResource effectData)
    {
        if (_healthComponent == null) return;

        float damage = effectData.DamagePerTick;
        if (effectData.IsPercentageBased)
        {
            damage = _healthComponent.MaxHealth * (damage / 100.0f);
        }

        _healthComponent.TakeDamage(damage);
        GD.Print($"{GetParent().Name} takes {damage.ToString("0.##")} damage from {effectData.Type}.");
    }
}
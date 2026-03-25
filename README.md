

KitchenChaos - Overcooked-Style Cooking Simulation Game
KitchenChaos is a fast-paced 3D cooking simulation game developed using the Unity Engine and C#. This project focuses on implementing clean code practices, robust game mechanics, and scalable systems similar to the popular game Overcooked.

 Key Features
Dynamic Interaction System: Players can pick up, drop, and interact with various kitchen counters (Cutting, Frying, Plating).

Cooking Pipeline: Complete workflow from raw ingredients to chopped, fried, and plated meals.

State Machine Logic: Robust logic for character movement, cutting animations, and cooking progress.

Scriptable Object System: Highly scalable architecture for adding new recipes, ingredients, and counter types.

UI & Feedback: Real-time progress bars, warning icons, and recipe UI for an immersive player experience.

 Technical Highlights
Design Patterns: Implemented Singleton Pattern for game managers and Observer Pattern (C# Events) for decoupled UI and sound systems.

Interfaces: Used IKitchenObjectParent interfaces to manage parent-child relationships between counters and ingredients.

Physics & Input: Custom character controller using Unity's Input System and physics-based movement.

 Getting Started
Clone the repository.

Open the project in Unity 2022.3 or later.

Open the GameScene and press Play.

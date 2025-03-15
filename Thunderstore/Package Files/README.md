# AddRunicLensToProcChain

When proc items hit an enemy, they add themselves to the proc chain mask, which prevents those items from occurring again in the same proc chain. Runic lens doesn't do this, in fact when it hits an enemy the game sees it as a fresh item hit from you without anything in the proc chain. This means that all your proc items can proc again, which can cause runic lens to proc again, and this cycle can happen over and over, potentially infinitely with enough luck/proc items.

This mod fixes all of that by making runic lens add itself to the proc chain mask the same as other proc items.

<sup><sub>Fun fact: the vanilla game checks for runic lens in the proc chain mask the same as other proc items, but because it's never added to the proc chain mask, the check is useless.</sub></sup>

## Before
![Risk_of_Rain_2_1mZWRcRUyj](https://github.com/user-attachments/assets/8e14e02e-33c3-4e24-89eb-b5e80d85217d)

## After
![Risk_of_Rain_2_OXpmKEZ7W1](https://github.com/user-attachments/assets/062bd247-9230-450a-8329-0377823e51a6)

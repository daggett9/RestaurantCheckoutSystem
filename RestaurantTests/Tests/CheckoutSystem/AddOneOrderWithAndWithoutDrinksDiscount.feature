Feature: AddOneOrderWithAndWithoutDrinksDiscount
Add new order for 1 starter, 2 mains and 2 drinks before 19:00
Verify the bill calculated
Update the order with 2 more mains 2 and drinks at 20:00
Verify the final bill calculated

@regression
@AddOneOrderWithAndWithoutDrinksDiscount
Scenario: Process one order with and without drinks discount
  Given customer placed new orders
    | OrderNumber | MenuCategoryType | Quantity | OrderTime |
    | Order1      | Starter          | 1        |  18:59    |
    | Order1      | Main             | 2        |  18:59    |
    | Order1      | Drink            | 2        |  18:59    |

  Then bill for the Order1 is correctly calculated

  When customer updated the existing orders
    | OrderNumber | MenuCategoryType | Quantity | OrderTime |
    | Order1      | Main             | 2        |  20:00    |
    | Order1      | Drink            | 2        |  20:00    |

  Then bill for the Order1 is correctly calculated
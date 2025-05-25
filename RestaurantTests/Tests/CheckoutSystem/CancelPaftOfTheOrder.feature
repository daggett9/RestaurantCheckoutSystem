Feature: CancelPaftOfTheOrder
Add new order for 1 starter, 4 mains and 4 drinks
Verify the bill calculated
Cancel 1 main and 1 drink from the order
Verify the final bill calculated

@regression
@CancelPaftOfTheOrder
Scenario: Process one order with and without drinks discount
  Given customer placed new orders
    | OrderNumber | MenuCategoryType | Quantity |
    | Order1      | Starter          | 1        |
    | Order1      | Main             | 4        |
    | Order1      | Drink            | 4        |

  Then bill for the Order1 is correctly calculated

  When customer updated the existing orders
    | OrderNumber | MenuCategoryType | Quantity |
    | Order1      | Main             | -1       |
    | Order1      | Drink            | -1       |

  Then bill for the Order1 is correctly calculated
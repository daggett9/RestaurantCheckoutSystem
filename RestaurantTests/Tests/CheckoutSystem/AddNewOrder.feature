Feature: AddNewOrder
Add new order for 4 starters, 4 mains and 4 drinks
Verify the bill calculated

@regression
@AddNewOrder
Scenario: Making a new order
  Given customer placed new orders
    | OrderNumber | MenuCategoryType | Quantity |
    | Order1      | Starter          | 4        |
    | Order1      | Main             | 4        |
    | Order1      | Drink            | 4        |

  Then bill for the Order1 is correctly calculated
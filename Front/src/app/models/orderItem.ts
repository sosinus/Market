import { Item } from './Item';

export class OrderItem {
    id?: number
    item_Id: number
    items_count: number
    item: Item
    item_Price: number
    order_id?: number
}
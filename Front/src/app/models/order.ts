import { OrderItem } from './orderItem';

export class Order {

    id?: number
    order_Date: string
    shipment_Date: string
    order_Number: number
    status: string
    orderItems: OrderItem[]
    customer_Id: number

}
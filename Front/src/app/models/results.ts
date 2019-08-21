export class GetJwtResult {
    token: string
    success: boolean
}
export class LoadPageResult {
    hasDefaultUser: boolean
}

export class CreateUserResult {
    message: string
    success: boolean
    isAlreadyExist: boolean
    other: boolean
}

export class ItemResult {
    success: boolean
}
export class AddOrderResult {
    success: boolean
}


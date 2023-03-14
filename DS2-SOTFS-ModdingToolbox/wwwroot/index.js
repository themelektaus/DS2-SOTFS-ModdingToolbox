const cursor = document.querySelector("#cursor")

document.body.addEventListener("mousemove", e =>
{
    cursor.style.left = e.x + "px"
    cursor.style.top = e.y + "px"
})

function scrollToBottom(element)
{
    element.scrollTo({ left: 0, top: element.scrollHeight, behavior: "smooth" })
}

function setClass(element, className, active)
{
    if (typeof element == "string")
        element = document.querySelector(element)

    if (active)
    {
        element.classList.add(className)
        return
    }

    element.classList.remove(className)
}
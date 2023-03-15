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
    let elements = []

    if (typeof element == "string")
        elements = document.querySelectorAll(element)
    else
        elements = [ element ]

    if (active)
    {
        for (const element of elements)
            element.classList.add(className)
        return
    }

    for (const element of elements)
        element.classList.remove(className)
}

function setContent(element, content)
{
    if (typeof element == "string")
        element = document.querySelector(element)

    element.innerHTML = content
}
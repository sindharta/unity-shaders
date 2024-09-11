namespace Shin.Core {
internal interface ICanRefresh {
    bool Refresh(); //return false if Refresh() failed, true otherwise
}

} //end namespace